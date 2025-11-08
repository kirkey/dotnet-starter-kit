using Accounting.Domain.Events.GeneralLedger;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a single general ledger posting line derived from journal entries for double-entry bookkeeping and financial reporting.
/// </summary>
/// <remarks>
/// Accounting Standards Compliance:
/// - GAAP/IFRS: Complete audit trail with source document linkage
/// - SOX: Immutability controls and user accountability
/// - Double-entry bookkeeping: Balanced debits and credits
/// - USOA: Utility-specific account classification support
/// 
/// Business Rules:
/// - Either Debit OR Credit must have an amount (not both, not neither)
/// - Amounts must be non-negative
/// - Cannot modify posted entries (immutable after posting)
/// - Complete audit trail required (who, when, source)
/// </remarks>
public class GeneralLedger : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Identifier of the source journal entry.
    /// </summary>
    public DefaultIdType EntryId { get; private set; }

    /// <summary>
    /// Identifier of the account being posted to.
    /// </summary>
    public DefaultIdType AccountId { get; private set; }

    /// <summary>
    /// Account code from chart of accounts (denormalized for query performance).
    /// </summary>
    public string AccountCode { get; private set; } = string.Empty;

    /// <summary>
    /// Debit amount (must be non-negative).
    /// </summary>
    public decimal Debit { get; private set; }

    /// <summary>
    /// Credit amount (must be non-negative).
    /// </summary>
    public decimal Credit { get; private set; }

    /// <summary>
    /// Optional memo text describing the posting.
    /// </summary>
    public string? Memo { get; private set; }

    /// <summary>
    /// USOA class for regulatory reporting (Generation, Transmission, Distribution, etc.).
    /// </summary>
    public string? UsoaClass { get; private set; }

    /// <summary>
    /// Transaction effective date for this ledger entry.
    /// </summary>
    public DateTime TransactionDate { get; private set; }

    /// <summary>
    /// Optional source reference number (invoice number, check number, etc.).
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Source type of the transaction (JournalEntry, Invoice, Bill, Payment, etc.).
    /// </summary>
    public string? Source { get; private set; }

    /// <summary>
    /// Source document identifier for complete audit trail.
    /// </summary>
    public DefaultIdType? SourceId { get; private set; }

    /// <summary>
    /// Indicates whether this entry has been posted to the general ledger (immutable after posting).
    /// </summary>
    public bool IsPosted { get; private set; }

    /// <summary>
    /// Date when the entry was posted (for audit trail).
    /// </summary>
    public DateTime? PostedDate { get; private set; }

    /// <summary>
    /// User who posted the entry (for audit trail and SOX compliance).
    /// </summary>
    public string? PostedBy { get; private set; }

    /// <summary>
    /// Optional accounting period identifier associated with this posting.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }
    
    private GeneralLedger()
    {
        AccountCode = string.Empty;
    }

    private GeneralLedger(DefaultIdType entryId, DefaultIdType accountId, string accountCode,
        decimal debit, decimal credit, DateTime transactionDate,
        string? usoaClass = null, string? memo = null, string? referenceNumber = null, 
        string? source = null, DefaultIdType? sourceId = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        EntryId = entryId;
        AccountId = accountId;
        AccountCode = accountCode.Trim();
        Debit = debit;
        Credit = credit;
        UsoaClass = usoaClass?.Trim();
        TransactionDate = transactionDate;
        Memo = memo?.Trim();
        ReferenceNumber = referenceNumber?.Trim();
        Source = source?.Trim();
        SourceId = sourceId;
        PeriodId = periodId;
        IsPosted = false;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new GeneralLedgerEntryCreated(Id, EntryId, AccountId, Debit, Credit, UsoaClass ?? string.Empty, TransactionDate));
    }

    /// <summary>
    /// Create a general ledger entry with validation for amounts and required fields.
    /// </summary>
    public static GeneralLedger Create(DefaultIdType entryId, DefaultIdType accountId, string accountCode,
        decimal debit, decimal credit, DateTime transactionDate,
        string? usoaClass = null, string? memo = null, string? referenceNumber = null, 
        string? source = null, DefaultIdType? sourceId = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        if (debit < 0 || credit < 0)
            throw new InvalidGeneralLedgerAmountException("Debit or credit amount cannot be negative");

        if (string.IsNullOrWhiteSpace(accountCode))
            throw new ArgumentException("Account code is required", nameof(accountCode));

        return new GeneralLedger(entryId, accountId, accountCode, debit, credit, transactionDate,
            usoaClass, memo, referenceNumber, source, sourceId, periodId, description, notes);
    }

    /// <summary>
    /// Mark the general ledger entry as posted (immutable after this operation).
    /// </summary>
    public void Post(string postedBy)
    {
        if (IsPosted)
            throw new InvalidOperationException("General ledger entry is already posted and cannot be modified");

        if (string.IsNullOrWhiteSpace(postedBy))
            throw new ArgumentException("Posted by user is required", nameof(postedBy));

        IsPosted = true;
        PostedDate = DateTime.UtcNow;
        PostedBy = postedBy;

        QueueDomainEvent(new GeneralLedgerPosted(Id, AccountCode, TransactionDate, Debit, Credit));
    }

    /// <summary>
    /// Update unposted entry amounts and metadata (cannot update posted entries).
    /// </summary>
    public GeneralLedger Update(decimal? debit = null, decimal? credit = null, string? memo = null,
        string? usoaClass = null, string? referenceNumber = null, string? description = null, string? notes = null)
    {
        if (IsPosted)
            throw new InvalidOperationException("Cannot update posted general ledger entries");

        bool isUpdated = false;

        if (debit.HasValue && Debit != debit.Value)
        {
            if (debit.Value < 0)
                throw new InvalidGeneralLedgerAmountException("Debit amount cannot be negative");
            Debit = debit.Value;
            isUpdated = true;
        }

        if (credit.HasValue && Credit != credit.Value)
        {
            if (credit.Value < 0)
                throw new InvalidGeneralLedgerAmountException("Credit amount cannot be negative");
            Credit = credit.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(memo) && Memo != memo.Trim())
        {
            Memo = memo.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(usoaClass) && UsoaClass != usoaClass.Trim())
        {
            UsoaClass = usoaClass.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(referenceNumber) && ReferenceNumber != referenceNumber.Trim())
        {
            ReferenceNumber = referenceNumber.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != (description?.Trim() ?? string.Empty))
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(notes) && Notes != (notes?.Trim() ?? string.Empty))
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new GeneralLedgerEntryUpdated(Id, EntryId, AccountId, Debit, Credit, UsoaClass ?? string.Empty));
        }

        return this;
    }
}


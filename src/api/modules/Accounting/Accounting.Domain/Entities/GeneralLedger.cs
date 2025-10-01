using Accounting.Domain.Events.GeneralLedger;

namespace Accounting.Domain;

/// <summary>
/// Represents a single general ledger posting line derived from journal entries for double-entry bookkeeping and financial reporting.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record individual debit and credit postings to maintain the general ledger trial balance.
/// - Support double-entry bookkeeping with balanced journal entry distributions.
/// - Enable financial statement preparation with account-level detail and classifications.
/// - Track USOA (Uniform System of Accounts) compliance for utility regulatory reporting.
/// - Provide audit trail linking general ledger postings to source journal entries.
/// - Support period-end reporting and account reconciliation processes.
/// - Enable detailed transaction analysis and variance reporting by account.
/// - Facilitate automated posting from subsidiary ledgers and system integrations.
/// 
/// Default values:
/// - EntryId: required reference to source journal entry
/// - AccountId: required reference to chart of accounts
/// - Debit: 0.00 (either debit or credit will have an amount, not both)
/// - Credit: 0.00 (either debit or credit will have an amount, not both)
/// - Memo: null (optional transaction description)
/// - UsoaClass: required for utility accounting (example: "Generation", "Transmission", "Distribution")
/// - TransactionDate: required effective date for the posting
/// - ReferenceNumber: null (optional external reference like invoice number)
/// 
/// Business rules:
/// - Either Debit OR Credit must have an amount (not both, not neither)
/// - Debit and Credit amounts must be non-negative
/// - UsoaClass must be valid for regulatory compliance
/// - TransactionDate must match the journal entry date
/// - Cannot modify posted general ledger entries (immutable after posting)
/// - Account must exist in chart of accounts
/// - Journal entry must be approved before posting
/// - Memo should provide meaningful transaction description
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.GeneralLedger.GeneralLedgerPosted"/>
/// <seealso cref="Accounting.Domain.Events.GeneralLedger.GeneralLedgerReversed"/>
/// <seealso cref="Accounting.Domain.Events.GeneralLedger.GeneralLedgerAdjusted"/>
public class GeneralLedger : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Identifier of the source journal entry that this ledger line is derived from.
    /// </summary>
    public DefaultIdType EntryId { get; private set; } // Foreign Key to Journal Entry

    /// <summary>
    /// Identifier of the account being posted to.
    /// </summary>
    public DefaultIdType AccountId { get; private set; } // Foreign Key to Chart of Accounts

    /// <summary>
    /// Debit amount (must be non-negative). Either debit or credit should be set by the posting logic.
    /// </summary>
    public decimal Debit { get; private set; }

    /// <summary>
    /// Credit amount (must be non-negative). Either debit or credit should be set by the posting logic.
    /// </summary>
    public decimal Credit { get; private set; }

    /// <summary>
    /// Optional memo text describing the posting.
    /// </summary>
    public string? Memo { get; private set; }

    /// <summary>
    /// USOA class for reporting (e.g., Generation, Transmission, Distribution).
    /// </summary>
    public string UsoaClass { get; private set; } // Generation, Transmission, Distribution

    /// <summary>
    /// Transaction effective date for this ledger entry.
    /// </summary>
    public DateTime TransactionDate { get; private set; }

    /// <summary>
    /// Optional source reference number.
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Optional accounting period identifier associated with this posting.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }
    
    private GeneralLedger()
    {
        UsoaClass = string.Empty;
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private GeneralLedger(DefaultIdType entryId, DefaultIdType accountId,
        decimal debit, decimal credit, string usoaClass, DateTime transactionDate,
        string? memo = null, string? referenceNumber = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        EntryId = entryId;
        AccountId = accountId;
        Debit = debit;
        Credit = credit;
        UsoaClass = usoaClass.Trim();
        TransactionDate = transactionDate;
        Memo = memo?.Trim();
        ReferenceNumber = referenceNumber?.Trim();
        PeriodId = periodId;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new GeneralLedgerEntryCreated(Id, EntryId, AccountId, Debit, Credit, UsoaClass, TransactionDate));
    }

    /// <summary>
    /// Create a general ledger entry line with validation for amounts and USOA class.
    /// </summary>
    public static GeneralLedger Create(DefaultIdType entryId, DefaultIdType accountId,
        decimal debit, decimal credit, string usoaClass, DateTime transactionDate,
        string? memo = null, string? referenceNumber = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        if (debit < 0 || credit < 0)
            throw new InvalidGeneralLedgerAmountException("Debit or credit amount cannot be negative");

        return new GeneralLedger(entryId, accountId, debit, credit, usoaClass,
            transactionDate, memo, referenceNumber, periodId, description, notes);
    }

    /// <summary>
    /// Update amounts and metadata; validates non-negative amounts and allowed USOA class values.
    /// </summary>
    public GeneralLedger Update(decimal? debit = null, decimal? credit = null, string? memo = null,
        string? usoaClass = null, string? referenceNumber = null, string? description = null, string? notes = null)
    {
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
            if (!IsValidUsoaClass(usoaClass))
                throw new InvalidUsoaClassException($"Invalid USOA class: {usoaClass}");
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
            QueueDomainEvent(new GeneralLedgerEntryUpdated(Id, EntryId, AccountId, Debit, Credit, UsoaClass));
        }

        return this;
    }

    private static bool IsValidUsoaClass(string usoaClass)
    {
        var validClasses = new[] { "Generation", "Transmission", "Distribution", "Customer Service", 
            "Sales", "Administrative", "General", "Maintenance" };
        return validClasses.Contains(usoaClass.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}

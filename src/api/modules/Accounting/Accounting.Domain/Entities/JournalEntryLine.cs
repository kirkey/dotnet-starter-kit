using Accounting.Domain.Events.JournalEntryLine;

namespace Accounting.Domain.Entities;

/// <summary>
/// A single detail line within a journal entry representing a debit or credit to a specific account.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record individual debits and credits for double-entry bookkeeping.
/// - Support detailed transaction tracking at the account level.
/// - Enable detailed audit trails for financial transactions.
/// - Facilitate account-level reporting and analysis.
///
/// Defaults:
/// - Memo: null (optional description up to 500 chars).
/// - Reference: null (optional reference up to 100 chars).
/// </remarks>
public class JournalEntryLine : AuditableEntity, IAggregateRoot
{
    private const int MaxMemoLength = 500;
    private const int MaxReferenceLength = 100;

    /// <summary>
    /// The parent Journal Entry aggregate identifier.
    /// </summary>
    public DefaultIdType JournalEntryId { get; private set; }

    /// <summary>
    /// The Chart of Account identifier this line applies to.
    /// </summary>
    public DefaultIdType AccountId { get; private set; }

    /// <summary>
    /// Debit amount for this line. Either debit or credit should be non-zero, not both.
    /// Example: 1000.00 for a $1,000 debit to an expense account.
    /// </summary>
    public decimal DebitAmount { get; private set; }

    /// <summary>
    /// Credit amount for this line. Either debit or credit should be non-zero, not both.
    /// Example: 1000.00 for a $1,000 credit to a revenue account.
    /// </summary>
    public decimal CreditAmount { get; private set; }

    /// <summary>
    /// Optional memo/description for this line item.
    /// Example: "Payment for consulting services - Invoice #12345".
    /// </summary>
    public string? Memo { get; private set; }

    /// <summary>
    /// Optional reference number or identifier for this line.
    /// Example: "CHECK-9876", "INV-12345". Used for reconciliation and lookup.
    /// </summary>
    public string? Reference { get; private set; }

    // EF Core
    private JournalEntryLine() { }

    private JournalEntryLine(DefaultIdType journalEntryId, DefaultIdType accountId,
        decimal debitAmount, decimal creditAmount, string? memo = null, string? reference = null)
    {
        if (journalEntryId == default)
            throw new ArgumentException("JournalEntryId is required.");

        if (accountId == default)
            throw new ArgumentException("AccountId is required.");

        // Validate that debit and credit are not both non-zero
        if (debitAmount != 0 && creditAmount != 0)
            throw new ArgumentException("A journal entry line cannot have both debit and credit amounts.");

        // Validate that at least one amount is non-zero
        if (debitAmount == 0 && creditAmount == 0)
            throw new ArgumentException("A journal entry line must have either a debit or credit amount.");

        // Validate non-negative amounts
        if (debitAmount < 0 || creditAmount < 0)
            throw new ArgumentException("Debit and credit amounts must be non-negative.");

        JournalEntryId = journalEntryId;
        AccountId = accountId;
        DebitAmount = debitAmount;
        CreditAmount = creditAmount;

        var trimmedMemo = memo?.Trim();
        if (!string.IsNullOrEmpty(trimmedMemo) && trimmedMemo.Length > MaxMemoLength)
            trimmedMemo = trimmedMemo.Substring(0, MaxMemoLength);
        Memo = trimmedMemo;

        var trimmedReference = reference?.Trim();
        if (!string.IsNullOrEmpty(trimmedReference) && trimmedReference.Length > MaxReferenceLength)
            trimmedReference = trimmedReference.Substring(0, MaxReferenceLength);
        Reference = trimmedReference;
        
        QueueDomainEvent(new JournalEntryLineCreated(Id, JournalEntryId, AccountId, DebitAmount, CreditAmount));
    }

    /// <summary>
    /// Factory to create a new journal entry line.
    /// </summary>
    public static JournalEntryLine Create(DefaultIdType journalEntryId, DefaultIdType accountId,
        decimal debitAmount, decimal creditAmount, string? memo = null, string? reference = null)
    {
        return new JournalEntryLine(journalEntryId, accountId, debitAmount, creditAmount, memo, reference);
    }

    /// <summary>
    /// Update amounts and/or memo for this line.
    /// </summary>
    public JournalEntryLine Update(decimal? debitAmount, decimal? creditAmount, string? memo, string? reference)
    {
        bool isUpdated = false;

        if (debitAmount.HasValue && DebitAmount != debitAmount.Value)
        {
            if (debitAmount.Value < 0)
                throw new ArgumentException("Debit amount must be non-negative.");
            
            // If setting debit, credit should be zero
            if (debitAmount.Value != 0 && CreditAmount != 0)
                throw new ArgumentException("Cannot have both debit and credit amounts.");
            
            DebitAmount = debitAmount.Value;
            isUpdated = true;
        }

        if (creditAmount.HasValue && CreditAmount != creditAmount.Value)
        {
            if (creditAmount.Value < 0)
                throw new ArgumentException("Credit amount must be non-negative.");
            
            // If setting credit, debit should be zero
            if (creditAmount.Value != 0 && DebitAmount != 0)
                throw new ArgumentException("Cannot have both debit and credit amounts.");
            
            CreditAmount = creditAmount.Value;
            isUpdated = true;
        }

        // Ensure at least one amount is non-zero
        if (DebitAmount == 0 && CreditAmount == 0)
            throw new ArgumentException("A journal entry line must have either a debit or credit amount.");

        if (memo != Memo)
        {
            var trimmedMemo = memo?.Trim();
            if (!string.IsNullOrEmpty(trimmedMemo) && trimmedMemo.Length > MaxMemoLength)
                trimmedMemo = trimmedMemo.Substring(0, MaxMemoLength);
            Memo = trimmedMemo;
            isUpdated = true;
        }

        if (reference != Reference)
        {
            var trimmedReference = reference?.Trim();
            if (!string.IsNullOrEmpty(trimmedReference) && trimmedReference.Length > MaxReferenceLength)
                trimmedReference = trimmedReference.Substring(0, MaxReferenceLength);
            Reference = trimmedReference;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new JournalEntryLineUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Queue deletion event for this journal entry line.
    /// </summary>
    public void Delete()
    {
        QueueDomainEvent(new JournalEntryLineDeleted(Id, JournalEntryId));
    }
}


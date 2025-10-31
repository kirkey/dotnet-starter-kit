using Accounting.Domain.Events.InterCompanyTransaction;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents an inter-company transaction between legal entities for tracking, reconciliation, and consolidation reporting.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track transactions between parent company and subsidiaries.
/// - Manage inter-company billing for shared services and allocations.
/// - Support inter-company loans and advances reconciliation.
/// - Enable automated inter-company elimination entries for consolidation.
/// - Track inter-company account balances for settlement processing.
/// - Support multi-entity accounting and reporting requirements.
/// - Facilitate inter-company profit elimination for consolidated statements.
/// - Enable audit trail for inter-company transaction flows.
/// 
/// Default values:
/// - TransactionNumber: required unique identifier (example: "IC-2025-001234")
/// - FromEntityId: required originating entity reference
/// - ToEntityId: required receiving entity reference
/// - TransactionDate: required transaction date (example: 2025-10-15)
/// - Amount: required transaction amount (example: 25000.00)
/// - TransactionType: required type (example: "Billing", "Loan", "Allocation")
/// - Status: "Pending" (new transactions start as pending)
/// - IsReconciled: false (not reconciled initially)
/// - ReconciliationDate: null (set when reconciled)
/// - FromAccountId: required GL account in originating entity
/// - ToAccountId: required GL account in receiving entity
/// 
/// Business rules:
/// - TransactionNumber must be unique within the system
/// - FromEntityId and ToEntityId must be different (no self transactions)
/// - Amount must be positive (sign determined by transaction type)
/// - Both entities must record matching transaction
/// - Status transitions: Pending → Matched → Reconciled → Closed
/// - Reconciliation requires matching amounts in both entities
/// - Inter-company balances must net to zero in consolidation
/// - Cannot delete reconciled transactions without reversal
/// - Automated elimination entries for consolidation reporting
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.InterCompanyTransaction.InterCompanyTransactionCreated"/>
/// <seealso cref="Accounting.Domain.Events.InterCompanyTransaction.InterCompanyTransactionUpdated"/>
/// <seealso cref="Accounting.Domain.Events.InterCompanyTransaction.InterCompanyTransactionMatched"/>
/// <seealso cref="Accounting.Domain.Events.InterCompanyTransaction.InterCompanyTransactionReconciled"/>
/// <seealso cref="Accounting.Domain.Events.InterCompanyTransaction.InterCompanyTransactionReversed"/>
public class InterCompanyTransaction : AuditableEntity, IAggregateRoot
{
    private const int MaxTransactionNumberLength = 50;
    private const int MaxTransactionTypeLength = 50;
    private const int MaxStatusLength = 32;
    private const int MaxFromEntityNameLength = 256;
    private const int MaxToEntityNameLength = 256;
    private const int MaxReferenceNumberLength = 100;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

    /// <summary>
    /// Unique inter-company transaction number.
    /// Example: "IC-2025-001234", "INTERCO-10001". Max length: 50.
    /// </summary>
    public string TransactionNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Identifier of the originating/sending entity.
    /// Example: links to legal entity performing service or advancing funds.
    /// Could reference an Entity table or use string identifier.
    /// </summary>
    public DefaultIdType FromEntityId { get; private set; }

    /// <summary>
    /// Name of the originating entity for display.
    /// Example: "ABC Parent Company Inc.". Max length: 256.
    /// Denormalized for reporting convenience.
    /// </summary>
    public string FromEntityName { get; private set; } = string.Empty;

    /// <summary>
    /// Identifier of the receiving entity.
    /// Example: links to legal entity receiving service or borrowing funds.
    /// </summary>
    public DefaultIdType ToEntityId { get; private set; }

    /// <summary>
    /// Name of the receiving entity for display.
    /// Example: "XYZ Subsidiary LLC". Max length: 256.
    /// Denormalized for reporting convenience.
    /// </summary>
    public string ToEntityName { get; private set; } = string.Empty;

    /// <summary>
    /// Date of the inter-company transaction.
    /// Example: 2025-10-15. Used for period allocation and reporting.
    /// </summary>
    public DateTime TransactionDate { get; private set; }

    /// <summary>
    /// Transaction amount.
    /// Example: 25000.00 for $25,000 inter-company charge.
    /// Must be positive. Sign interpreted based on transaction type.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Type of inter-company transaction.
    /// Values: "Billing", "Loan", "Advance", "Allocation", "Dividend", "CapitalContribution", "Settlement", "Other".
    /// Example: "Billing" for shared services charge. Max length: 50.
    /// </summary>
    public string TransactionType { get; private set; } = string.Empty;

    /// <summary>
    /// Current status of the inter-company transaction.
    /// Values: "Pending", "Matched", "Reconciled", "Disputed", "Reversed", "Closed".
    /// Default: "Pending". Max length: 32.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// GL account in the originating entity's books.
    /// Links to ChartOfAccount entity. Example: Inter-Company Receivable account.
    /// </summary>
    public DefaultIdType FromAccountId { get; private set; }

    /// <summary>
    /// GL account in the receiving entity's books.
    /// Links to ChartOfAccount entity. Example: Inter-Company Payable account.
    /// </summary>
    public DefaultIdType ToAccountId { get; private set; }

    /// <summary>
    /// Whether the transaction has been reconciled between entities.
    /// Default: false. True when both sides confirm and amounts match.
    /// </summary>
    public bool IsReconciled { get; private set; }

    /// <summary>
    /// Date when the transaction was reconciled.
    /// Example: 2025-10-20. Null until reconciliation completed.
    /// </summary>
    public DateTime? ReconciliationDate { get; private set; }

    /// <summary>
    /// Person or system that performed reconciliation.
    /// Example: "john.doe@company.com", "Automated System". Max length: 256.
    /// </summary>
    public string? ReconciledBy { get; private set; }

    /// <summary>
    /// Optional reference to matching transaction in counterparty entity.
    /// Links to another InterCompanyTransaction record for bi-directional tracking.
    /// </summary>
    public DefaultIdType? MatchingTransactionId { get; private set; }

    /// <summary>
    /// External reference number from source document.
    /// Example: "INV-12345", "LOAN-2025-001". Max length: 100.
    /// Used to trace back to original transaction source.
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Optional journal entry in originating entity.
    /// Links to JournalEntry entity for GL integration.
    /// </summary>
    public DefaultIdType? FromJournalEntryId { get; private set; }

    /// <summary>
    /// Optional journal entry in receiving entity.
    /// Links to JournalEntry entity for GL integration.
    /// </summary>
    public DefaultIdType? ToJournalEntryId { get; private set; }

    /// <summary>
    /// Date the transaction is due for settlement/payment.
    /// Example: 2025-11-14 for Net 30 terms. Null if not applicable.
    /// </summary>
    public DateTime? DueDate { get; private set; }

    /// <summary>
    /// Date the transaction was settled/paid.
    /// Example: 2025-11-10 if paid before due date. Null if not settled.
    /// </summary>
    public DateTime? SettlementDate { get; private set; }

    /// <summary>
    /// Optional accounting period for reporting.
    /// Links to AccountingPeriod entity for period-based tracking.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    /// <summary>
    /// Whether this transaction requires elimination in consolidation.
    /// Default: true. False for transactions that should not be eliminated.
    /// </summary>
    public bool RequiresElimination { get; private set; }

    /// <summary>
    /// Whether elimination entry has been posted for consolidation.
    /// Default: false. True after consolidation elimination entry created.
    /// </summary>
    public bool IsEliminated { get; private set; }

    /// <summary>
    /// Date when elimination entry was posted.
    /// Example: 2025-10-31 at period-end close. Null until eliminated.
    /// </summary>
    public DateTime? EliminationDate { get; private set; }

    // Parameterless constructor for EF Core
    private InterCompanyTransaction()
    {
        TransactionNumber = string.Empty;
        FromEntityName = string.Empty;
        ToEntityName = string.Empty;
        TransactionType = string.Empty;
        Status = "Pending";
    }

    private InterCompanyTransaction(string transactionNumber, DefaultIdType fromEntityId,
        string fromEntityName, DefaultIdType toEntityId, string toEntityName,
        DateTime transactionDate, decimal amount, string transactionType,
        DefaultIdType fromAccountId, DefaultIdType toAccountId,
        string? referenceNumber = null, DateTime? dueDate = null,
        bool requiresElimination = true, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(transactionNumber))
            throw new ArgumentException("Transaction number is required", nameof(transactionNumber));

        if (transactionNumber.Length > MaxTransactionNumberLength)
            throw new ArgumentException($"Transaction number cannot exceed {MaxTransactionNumberLength} characters", nameof(transactionNumber));

        if (fromEntityId == toEntityId)
            throw new ArgumentException("From entity and to entity must be different", nameof(toEntityId));

        if (string.IsNullOrWhiteSpace(fromEntityName))
            throw new ArgumentException("From entity name is required", nameof(fromEntityName));

        if (string.IsNullOrWhiteSpace(toEntityName))
            throw new ArgumentException("To entity name is required", nameof(toEntityName));

        if (amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        if (string.IsNullOrWhiteSpace(transactionType))
            throw new ArgumentException("Transaction type is required", nameof(transactionType));

        if (dueDate.HasValue && dueDate.Value < transactionDate)
            throw new ArgumentException("Due date cannot be before transaction date", nameof(dueDate));

        TransactionNumber = transactionNumber.Trim();
        Name = transactionNumber.Trim(); // For AuditableEntity compatibility
        FromEntityId = fromEntityId;
        FromEntityName = fromEntityName.Trim();
        ToEntityId = toEntityId;
        ToEntityName = toEntityName.Trim();
        TransactionDate = transactionDate;
        Amount = amount;
        TransactionType = transactionType.Trim();
        Status = "Pending";
        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
        IsReconciled = false;
        ReferenceNumber = referenceNumber?.Trim();
        DueDate = dueDate;
        RequiresElimination = requiresElimination;
        IsEliminated = false;
        PeriodId = periodId;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new InterCompanyTransactionCreated(Id, TransactionNumber, FromEntityName, ToEntityName, Amount, TransactionDate, Description, Notes));
    }

    /// <summary>
    /// Factory method to create a new inter-company transaction with validation.
    /// </summary>
    public static InterCompanyTransaction Create(string transactionNumber, DefaultIdType fromEntityId,
        string fromEntityName, DefaultIdType toEntityId, string toEntityName,
        DateTime transactionDate, decimal amount, string transactionType,
        DefaultIdType fromAccountId, DefaultIdType toAccountId,
        string? referenceNumber = null, DateTime? dueDate = null,
        bool requiresElimination = true, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        return new InterCompanyTransaction(transactionNumber, fromEntityId, fromEntityName,
            toEntityId, toEntityName, transactionDate, amount, transactionType,
            fromAccountId, toAccountId, referenceNumber, dueDate, requiresElimination,
            periodId, description, notes);
    }

    /// <summary>
    /// Update inter-company transaction details; not allowed when reconciled.
    /// </summary>
    public InterCompanyTransaction Update(decimal? amount = null, DateTime? dueDate = null,
        string? referenceNumber = null, string? description = null, string? notes = null)
    {
        if (IsReconciled)
            throw new InvalidOperationException("Cannot modify reconciled inter-company transaction");

        bool isUpdated = false;

        if (amount.HasValue && Amount != amount.Value)
        {
            if (amount.Value <= 0)
                throw new ArgumentException("Amount must be positive");
            Amount = amount.Value;
            isUpdated = true;
        }

        if (dueDate.HasValue && DueDate != dueDate.Value)
        {
            if (dueDate.Value < TransactionDate)
                throw new ArgumentException("Due date cannot be before transaction date");
            DueDate = dueDate.Value;
            isUpdated = true;
        }

        if (referenceNumber != ReferenceNumber)
        {
            ReferenceNumber = referenceNumber?.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InterCompanyTransactionUpdated(Id, TransactionNumber, Amount, Description, Notes));
        }

        return this;
    }

    /// <summary>
    /// Match with counterparty transaction.
    /// </summary>
    public InterCompanyTransaction MatchWith(DefaultIdType matchingTransactionId)
    {
        if (Status == "Matched" || Status == "Reconciled")
            throw new InvalidOperationException($"Transaction is already {Status.ToLower()}");

        MatchingTransactionId = matchingTransactionId;
        Status = "Matched";

        QueueDomainEvent(new InterCompanyTransactionMatched(Id, TransactionNumber, FromEntityName, ToEntityName, matchingTransactionId));
        return this;
    }

    /// <summary>
    /// Reconcile the inter-company transaction.
    /// </summary>
    public InterCompanyTransaction Reconcile(string reconciledBy)
    {
        if (string.IsNullOrWhiteSpace(reconciledBy))
            throw new ArgumentException("Reconciled by information is required", nameof(reconciledBy));

        if (Status != "Matched")
            throw new InvalidOperationException("Transaction must be matched before reconciliation");

        IsReconciled = true;
        ReconciliationDate = DateTime.UtcNow;
        ReconciledBy = reconciledBy.Trim();
        Status = "Reconciled";

        QueueDomainEvent(new InterCompanyTransactionReconciled(Id, TransactionNumber, FromEntityName, ToEntityName, ReconciledBy, ReconciliationDate.Value));
        return this;
    }

    /// <summary>
    /// Mark as disputed with reason.
    /// </summary>
    public InterCompanyTransaction MarkDisputed(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Dispute reason is required", nameof(reason));

        Status = "Disputed";
        Notes = $"{Notes}\n\nDisputed: {reason.Trim()}".Trim();

        QueueDomainEvent(new InterCompanyTransactionDisputed(Id, TransactionNumber, FromEntityName, ToEntityName, reason));
        return this;
    }

    /// <summary>
    /// Resolve dispute and return to pending status.
    /// </summary>
    public InterCompanyTransaction ResolveDispute()
    {
        if (Status != "Disputed")
            throw new InvalidOperationException("Transaction is not disputed");

        Status = "Pending";

        QueueDomainEvent(new InterCompanyTransactionDisputeResolved(Id, TransactionNumber));
        return this;
    }

    /// <summary>
    /// Record settlement/payment of the transaction.
    /// </summary>
    public InterCompanyTransaction RecordSettlement(DateTime settlementDate)
    {
        if (!IsReconciled)
            throw new InvalidOperationException("Transaction must be reconciled before settlement");

        SettlementDate = settlementDate;

        QueueDomainEvent(new InterCompanyTransactionSettled(Id, TransactionNumber, settlementDate));
        return this;
    }

    /// <summary>
    /// Post elimination entry for consolidation.
    /// </summary>
    public InterCompanyTransaction PostElimination()
    {
        if (!RequiresElimination)
            throw new InvalidOperationException("Transaction does not require elimination");

        if (IsEliminated)
            throw new InvalidOperationException("Elimination entry already posted");

        IsEliminated = true;
        EliminationDate = DateTime.UtcNow;

        QueueDomainEvent(new InterCompanyTransactionEliminated(Id, TransactionNumber, EliminationDate.Value));
        return this;
    }

    /// <summary>
    /// Reverse the inter-company transaction.
    /// </summary>
    public InterCompanyTransaction Reverse(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reversal reason is required", nameof(reason));

        if (IsEliminated)
            throw new InvalidOperationException("Cannot reverse eliminated transaction");

        Status = "Reversed";
        Notes = $"{Notes}\n\nReversed: {reason.Trim()}".Trim();

        QueueDomainEvent(new InterCompanyTransactionReversed(Id, TransactionNumber, reason));
        return this;
    }

    /// <summary>
    /// Link journal entries for both entities.
    /// </summary>
    public InterCompanyTransaction LinkJournalEntries(DefaultIdType? fromJournalEntryId, DefaultIdType? toJournalEntryId)
    {
        FromJournalEntryId = fromJournalEntryId;
        ToJournalEntryId = toJournalEntryId;
        return this;
    }

    /// <summary>
    /// Close the inter-company transaction.
    /// </summary>
    public InterCompanyTransaction Close()
    {
        if (!IsReconciled)
            throw new InvalidOperationException("Can only close reconciled transactions");

        Status = "Closed";

        QueueDomainEvent(new InterCompanyTransactionClosed(Id, TransactionNumber));
        return this;
    }

    /// <summary>
    /// Whether the transaction is overdue for settlement.
    /// </summary>
    public bool IsOverdue => DueDate.HasValue && !SettlementDate.HasValue && DateTime.UtcNow.Date > DueDate.Value.Date;

    /// <summary>
    /// Days past due for settlement.
    /// </summary>
    public int DaysPastDue => IsOverdue ? (DateTime.UtcNow.Date - DueDate!.Value.Date).Days : 0;
}


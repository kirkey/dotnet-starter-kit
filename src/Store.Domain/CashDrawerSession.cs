namespace Store.Domain;

/// <summary>
/// Represents an operator cash drawer session (open/close) with tracked cash movements.
/// </summary>
/// <remarks>
/// Use cases:
/// - Open a drawer with a starting float and record cash movements during a shift.
/// - Close a drawer at end of shift and reconcile expected vs actual cash.
/// - Audit transactions for discrepancies.
/// </remarks>
/// <seealso cref="Store.Domain.Events.CashDrawerSessionOpened"/>
/// <seealso cref="Store.Domain.Events.CashDrawerTransactionAdded"/>
/// <seealso cref="Store.Domain.Events.CashDrawerSessionClosed"/>
/// <seealso cref="Store.Domain.Exceptions.CashDrawer.CashDrawerSessionNotFoundException"/>
public sealed class CashDrawerSession : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Human-friendly session number or identifier.
    /// Example: "CDS-2025-09-001".
    /// </summary>
    public string SessionNumber { get; private set; } = default!;

    /// <summary>
    /// Identifier for the operator/employee assigned to the drawer.
    /// Example: "EMP-1001".
    /// </summary>
    public string OperatorId { get; private set; } = default!;

    /// <summary>
    /// Date and time when the session was opened. Defaults to current UTC if unspecified.
    /// Example: 2025-09-18T08:00:00Z.
    /// </summary>
    public DateTime OpenedOn { get; private set; }

    /// <summary>
    /// Date and time when the session was closed. Null until closed.
    /// Example: null when still open.
    /// </summary>
    public DateTime? ClosedOn { get; private set; }

    /// <summary>
    /// Starting float placed in the drawer at open.
    /// Example: 200.00 (must be &gt;= 0).
    /// </summary>
    public decimal OpeningFloat { get; private set; }

    /// <summary>
    /// Amount counted at close. Example: 1150.00. Default 0.00 until closing.
    /// </summary>
    public decimal ClosingAmount { get; private set; }

    /// <summary>
    /// Current session status. Allowed: Open, Closed. Default: Open.
    /// </summary>
    public string Status { get; private set; } = "Open"; // Open, Closed

    /// <summary>
    /// Cash-related transactions (IN/OUT/ADJUSTMENT) recorded during the session.
    /// Example count: 0 at session open.
    /// </summary>
    public ICollection<CashDrawerTransaction> Transactions { get; private set; } = new List<CashDrawerTransaction>();

    private CashDrawerSession() { }

    private CashDrawerSession(DefaultIdType id, string sessionNumber, string operatorId, DateTime openedOn, decimal openingFloat)
    {
        if (string.IsNullOrWhiteSpace(sessionNumber)) throw new ArgumentException("SessionNumber is required", nameof(sessionNumber));
        if (sessionNumber.Length > 100) throw new ArgumentException("SessionNumber must not exceed 100 characters", nameof(sessionNumber));
        if (string.IsNullOrWhiteSpace(operatorId)) throw new ArgumentException("OperatorId is required", nameof(operatorId));
        if (operatorId.Length > 100) throw new ArgumentException("OperatorId must not exceed 100 characters", nameof(operatorId));
        if (openingFloat < 0m) throw new ArgumentException("OpeningFloat cannot be negative", nameof(openingFloat));

        Id = id;
        SessionNumber = sessionNumber;
        OperatorId = operatorId;
        OpenedOn = openedOn == default ? DateTime.UtcNow : openedOn;
        OpeningFloat = openingFloat;
        Status = "Open";
        QueueDomainEvent(new CashDrawerSessionOpened { Session = this });
    }

    /// <summary>
    /// Opens a new cash drawer session in Open status.
    /// </summary>
    /// <param name="sessionNumber">Example: "CDS-2025-09-001".</param>
    /// <param name="operatorId">Operator/employee id. Example: "EMP-1001".</param>
    /// <param name="openedOn">Open timestamp. Defaults to now when unspecified.</param>
    /// <param name="openingFloat">Starting float. Example: 200.00.</param>
    public static CashDrawerSession Open(string sessionNumber, string operatorId, DateTime openedOn, decimal openingFloat)
        => new(DefaultIdType.NewGuid(), sessionNumber, operatorId, openedOn, openingFloat);

    /// <summary>
    /// Adds a cash movement to the session (e.g., IN, OUT, ADJUSTMENT).
    /// Increases auditability and supports reconciliation.
    /// </summary>
    /// <param name="type">Example: "IN" or "OUT".</param>
    /// <param name="amount">Positive amount. Example: 50.00.</param>
    /// <param name="reason">Optional reason. Example: "Start float top-up".</param>
    public CashDrawerSession AddTransaction(string type, decimal amount, string? reason = null)
    {
        EnsureOpen();
        if (amount <= 0m) throw new ArgumentException("Amount must be positive", nameof(amount));
        var tx = CashDrawerTransaction.Create(Id, type, amount, reason);
        Transactions.Add(tx);
        QueueDomainEvent(new CashDrawerTransactionAdded { Session = this, Transaction = tx });
        return this;
    }

    /// <summary>
    /// Closes the session with a counted closing amount and emits a close event.
    /// </summary>
    /// <param name="closedOn">Close timestamp. Defaults to now when unspecified.</param>
    /// <param name="closingAmount">Counted cash amount at close. Example: 1150.00.</param>
    public CashDrawerSession Close(DateTime closedOn, decimal closingAmount)
    {
        EnsureOpen();
        if (closingAmount < 0m) throw new ArgumentException("ClosingAmount cannot be negative", nameof(closingAmount));
        ClosedOn = closedOn == default ? DateTime.UtcNow : closedOn;
        ClosingAmount = closingAmount;
        Status = "Closed";
        QueueDomainEvent(new CashDrawerSessionClosed { Session = this });
        return this;
    }

    private void EnsureOpen()
    {
        if (!string.Equals(Status, "Open", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Session is not open.");
    }
}

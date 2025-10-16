using Accounting.Domain.Events.Check;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a physical or electronic check that can be registered in the system and later used for payments.
/// </summary>
/// <remarks>
/// Use cases:
/// - Register check books and individual checks in the system for tracking.
/// - Assign checks to vendor payments or expenses when issuing payments.
/// - Track check status from available through cleared or voided.
/// - Support check reconciliation with bank statements.
/// - Prevent duplicate check usage and maintain audit trail.
/// - Enable check printing and signature tracking.
/// - Support stop payment and void operations.
/// 
/// Default values:
/// - Status: Available (new checks are available for use)
/// - IssuedDate: null (set when check is issued)
/// - ClearedDate: null (set when check clears the bank)
/// - Amount: null (set when check is issued)
/// - IsVoided: false
/// - IsPrinted: false
/// 
/// Business rules:
/// - Check number must be unique within a bank account
/// - Available checks can be issued for payments
/// - Issued checks cannot be reused
/// - Amount must be positive when issuing
/// - Voided checks cannot be issued
/// - Cleared checks cannot be voided (must use reversal)
/// - Check status progression: Available → Issued → Cleared
/// - Alternative paths: Available → Void, Issued → Void, Issued → Stale
/// </remarks>
public class Check : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique check number printed on the check.
    /// Example: "1001", "CHK-2025-001". Must be unique per bank account.
    /// </summary>
    public string CheckNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Bank account code from which the check is drawn.
    /// Example: "102" for checking account. Links to ChartOfAccount.
    /// </summary>
    public string BankAccountCode { get; private set; } = string.Empty;

    /// <summary>
    /// Bank account name for display purposes.
    /// Example: "Operating Checking Account", "Payroll Account".
    /// </summary>
    public string? BankAccountName { get; private set; }

    /// <summary>
    /// Bank ID that the check is associated with.
    /// Links to the Bank entity for bank-level information.
    /// </summary>
    public DefaultIdType? BankId { get; private set; }

    /// <summary>
    /// Bank name for display purposes.
    /// Example: "Chase Bank", "Bank of America".
    /// </summary>
    public string? BankName { get; private set; }

    /// <summary>
    /// Check status: Available, Issued, Cleared, Void, Stale, StopPayment.
    /// Default: Available.
    /// </summary>
    public string Status { get; private set; } = "Available";

    /// <summary>
    /// Amount written on the check when issued. Null for available checks.
    /// Example: 1500.00, 2345.67. Must be positive when set.
    /// </summary>
    public decimal? Amount { get; private set; }

    /// <summary>
    /// Payee name written on the check.
    /// Example: "ABC Suppliers Inc.", "John Doe". Set when check is issued.
    /// </summary>
    public string? PayeeName { get; private set; }

    /// <summary>
    /// Optional vendor ID if check is issued to a vendor.
    /// Links to Vendor entity for payment tracking.
    /// </summary>
    public DefaultIdType? VendorId { get; private set; }

    /// <summary>
    /// Optional payee ID if check is issued to a payee.
    /// Links to Payee entity for payment tracking.
    /// </summary>
    public DefaultIdType? PayeeId { get; private set; }

    /// <summary>
    /// Date when the check was issued/written.
    /// Example: 2025-10-15. Set when check status changes to Issued.
    /// </summary>
    public DateTime? IssuedDate { get; private set; }

    /// <summary>
    /// Date when the check cleared the bank.
    /// Example: 2025-10-20. Set during bank reconciliation.
    /// </summary>
    public DateTime? ClearedDate { get; private set; }

    /// <summary>
    /// Date when the check was voided.
    /// Example: 2025-10-16. Set when check is voided.
    /// </summary>
    public DateTime? VoidedDate { get; private set; }

    /// <summary>
    /// Reason for voiding the check.
    /// Example: "Check lost", "Incorrect amount", "Duplicate payment".
    /// </summary>
    public string? VoidReason { get; private set; }

    /// <summary>
    /// Reference to associated payment transaction ID.
    /// Links check to Payment entity when used for payment.
    /// </summary>
    public DefaultIdType? PaymentId { get; private set; }

    /// <summary>
    /// Reference to associated expense transaction ID.
    /// Links check to expense entry if applicable.
    /// </summary>
    public DefaultIdType? ExpenseId { get; private set; }

    /// <summary>
    /// Memo or note written on the check.
    /// Example: "Invoice #12345 payment", "Office supplies".
    /// </summary>
    public string? Memo { get; private set; }

    /// <summary>
    /// Whether the check has been printed.
    /// Used for check printing management. Default: false.
    /// </summary>
    public bool IsPrinted { get; private set; }

    /// <summary>
    /// Date when the check was printed.
    /// Example: 2025-10-15. Set when check is printed.
    /// </summary>
    public DateTime? PrintedDate { get; private set; }

    /// <summary>
    /// User who printed the check.
    /// Example: "john.doe@company.com". Audit trail for printed checks.
    /// </summary>
    public string? PrintedBy { get; private set; }

    /// <summary>
    /// Whether a stop payment has been requested.
    /// Example: true if check is lost or needs to be cancelled. Default: false.
    /// </summary>
    public bool IsStopPayment { get; private set; }

    /// <summary>
    /// Date when stop payment was requested.
    /// Example: 2025-10-16. Set when stop payment is issued.
    /// </summary>
    public DateTime? StopPaymentDate { get; private set; }

    /// <summary>
    /// Reason for stop payment request.
    /// Example: "Check stolen", "Payment cancelled by vendor".
    /// </summary>
    public string? StopPaymentReason { get; private set; }

    private Check()
    {
    }

    private Check(string checkNumber, string bankAccountCode, string? bankAccountName, DefaultIdType? bankId, string? bankName, string? description = null, string? notes = null)
    {
        CheckNumber = checkNumber.Trim();
        BankAccountCode = bankAccountCode.Trim();
        BankAccountName = bankAccountName?.Trim();
        BankId = bankId;
        BankName = bankName?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
        Status = "Available";
        IsPrinted = false;
        IsStopPayment = false;

        QueueDomainEvent(new CheckRegistered(Id, CheckNumber, BankAccountCode, Status));
    }

    /// <summary>
    /// Register a new check in the system with available status.
    /// </summary>
    /// <param name="checkNumber">Unique check number.</param>
    /// <param name="bankAccountCode">Bank account code from chart of accounts.</param>
    /// <param name="bankAccountName">Bank account name for display.</param>
    /// <param name="bankId">Optional bank ID from the Bank entity.</param>
    /// <param name="bankName">Optional bank name for display.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="notes">Optional notes.</param>
    /// <returns>New Check entity.</returns>
    /// <exception cref="ArgumentException">Thrown when required fields are missing.</exception>
    public static Check Create(string checkNumber, string bankAccountCode, string? bankAccountName = null, DefaultIdType? bankId = null, string? bankName = null, string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(checkNumber))
            throw new ArgumentException("Check number is required.");
        if (string.IsNullOrWhiteSpace(bankAccountCode))
            throw new ArgumentException("Bank account code is required.");

        return new Check(checkNumber, bankAccountCode, bankAccountName, bankId, bankName, description, notes);
    }

    /// <summary>
    /// Issue the check for payment to a payee.
    /// </summary>
    /// <param name="amount">Check amount (must be positive).</param>
    /// <param name="payeeName">Name of the payee.</param>
    /// <param name="issuedDate">Date the check was issued.</param>
    /// <param name="payeeId">Optional payee ID.</param>
    /// <param name="vendorId">Optional vendor ID.</param>
    /// <param name="paymentId">Optional payment transaction ID.</param>
    /// <param name="expenseId">Optional expense transaction ID.</param>
    /// <param name="memo">Optional memo for the check.</param>
    /// <returns>Updated Check entity.</returns>
    /// <exception cref="ArgumentException">Thrown when amount is not positive or payee name is missing.</exception>
    /// <exception cref="InvalidOperationException">Thrown when check is not available for issue.</exception>
    public Check Issue(decimal amount, string payeeName, DateTime issuedDate, DefaultIdType? payeeId = null, DefaultIdType? vendorId = null, DefaultIdType? paymentId = null, DefaultIdType? expenseId = null, string? memo = null)
    {
        if (Status != "Available")
            throw new InvalidOperationException($"Cannot issue check with status '{Status}'. Only available checks can be issued.");
        if (amount <= 0)
            throw new ArgumentException("Check amount must be positive.");
        if (string.IsNullOrWhiteSpace(payeeName))
            throw new ArgumentException("Payee name is required.");
        if (IsVoided())
            throw new InvalidOperationException("Cannot issue a voided check.");

        Amount = amount;
        PayeeName = payeeName.Trim();
        IssuedDate = issuedDate;
        PayeeId = payeeId;
        VendorId = vendorId;
        PaymentId = paymentId;
        ExpenseId = expenseId;
        Memo = memo?.Trim();
        Status = "Issued";

        QueueDomainEvent(new CheckIssued(Id, CheckNumber, amount, payeeName, issuedDate, vendorId, payeeId));
        return this;
    }

    /// <summary>
    /// Mark the check as printed.
    /// </summary>
    /// <param name="printedBy">User who printed the check.</param>
    /// <param name="printedDate">Date when printed (defaults to UTC now).</param>
    /// <returns>Updated Check entity.</returns>
    public Check MarkAsPrinted(string printedBy, DateTime? printedDate = null)
    {
        if (string.IsNullOrWhiteSpace(printedBy))
            throw new ArgumentException("Printed by user is required.");

        IsPrinted = true;
        PrintedDate = printedDate ?? DateTime.UtcNow;
        PrintedBy = printedBy.Trim();

        QueueDomainEvent(new CheckPrinted(Id, CheckNumber, PrintedDate.Value, printedBy));
        return this;
    }

    /// <summary>
    /// Mark the check as cleared through bank reconciliation.
    /// </summary>
    /// <param name="clearedDate">Date when the check cleared.</param>
    /// <returns>Updated Check entity.</returns>
    /// <exception cref="InvalidOperationException">Thrown when check is not issued or already cleared.</exception>
    public Check MarkAsCleared(DateTime clearedDate)
    {
        if (Status != "Issued")
            throw new InvalidOperationException($"Cannot clear check with status '{Status}'. Only issued checks can be cleared.");
        if (ClearedDate.HasValue)
            throw new InvalidOperationException("Check is already cleared.");

        ClearedDate = clearedDate;
        Status = "Cleared";

        QueueDomainEvent(new CheckCleared(Id, CheckNumber, Amount ?? 0, clearedDate));
        return this;
    }

    /// <summary>
    /// Void the check with reason.
    /// </summary>
    /// <param name="voidReason">Reason for voiding.</param>
    /// <param name="voidedDate">Date when voided (defaults to UTC now).</param>
    /// <returns>Updated Check entity.</returns>
    /// <exception cref="InvalidOperationException">Thrown when check cannot be voided.</exception>
    public Check Void(string voidReason, DateTime? voidedDate = null)
    {
        if (Status == "Cleared")
            throw new InvalidOperationException("Cannot void a cleared check. Use reversal journal entry instead.");
        if (IsVoided())
            throw new InvalidOperationException("Check is already voided.");
        if (string.IsNullOrWhiteSpace(voidReason))
            throw new ArgumentException("Void reason is required.");

        VoidedDate = voidedDate ?? DateTime.UtcNow;
        VoidReason = voidReason.Trim();
        Status = "Void";

        QueueDomainEvent(new CheckVoided(Id, CheckNumber, VoidedDate.Value, voidReason));
        return this;
    }

    /// <summary>
    /// Request stop payment on the check.
    /// </summary>
    /// <param name="stopPaymentReason">Reason for stop payment.</param>
    /// <param name="stopPaymentDate">Date of stop payment request (defaults to UTC now).</param>
    /// <returns>Updated Check entity.</returns>
    /// <exception cref="InvalidOperationException">Thrown when check cannot have stop payment.</exception>
    public Check StopPayment(string stopPaymentReason, DateTime? stopPaymentDate = null)
    {
        if (Status == "Cleared")
            throw new InvalidOperationException("Cannot stop payment on a cleared check.");
        if (Status == "Void")
            throw new InvalidOperationException("Cannot stop payment on a voided check.");
        if (IsStopPayment)
            throw new InvalidOperationException("Stop payment already requested for this check.");
        if (string.IsNullOrWhiteSpace(stopPaymentReason))
            throw new ArgumentException("Stop payment reason is required.");

        IsStopPayment = true;
        StopPaymentDate = stopPaymentDate ?? DateTime.UtcNow;
        StopPaymentReason = stopPaymentReason.Trim();
        Status = "StopPayment";

        QueueDomainEvent(new CheckStopPaymentRequested(Id, CheckNumber, StopPaymentDate.Value, stopPaymentReason));
        return this;
    }

    /// <summary>
    /// Mark the check as stale (outstanding for too long).
    /// </summary>
    /// <returns>Updated Check entity.</returns>
    /// <exception cref="InvalidOperationException">Thrown when check cannot be marked as stale.</exception>
    public Check MarkAsStale()
    {
        if (Status != "Issued")
            throw new InvalidOperationException("Only issued checks can be marked as stale.");

        Status = "Stale";
        QueueDomainEvent(new CheckMarkedAsStale(Id, CheckNumber, DateTime.UtcNow));
        return this;
    }

    /// <summary>
    /// Update check details (for available checks only).
    /// </summary>
    /// <param name="bankAccountCode">Bank account code.</param>
    /// <param name="bankAccountName">Bank account name.</param>
    /// <param name="bankId">Bank ID.</param>
    /// <param name="bankName">Bank name.</param>
    /// <param name="description">Description.</param>
    /// <param name="notes">Notes.</param>
    /// <returns>Updated Check entity.</returns>
    /// <exception cref="InvalidOperationException">Thrown when check is not available.</exception>
    public Check Update(string? bankAccountCode = null, string? bankAccountName = null, DefaultIdType? bankId = null, string? bankName = null, string? description = null, string? notes = null)
    {
        if (Status != "Available")
            throw new InvalidOperationException("Only available checks can be updated.");

        if (!string.IsNullOrWhiteSpace(bankAccountCode))
            BankAccountCode = bankAccountCode.Trim();
        if (!string.IsNullOrWhiteSpace(bankAccountName))
            BankAccountName = bankAccountName.Trim();
        if (bankId != null)
            BankId = bankId;
        if (!string.IsNullOrWhiteSpace(bankName))
            BankName = bankName.Trim();
        if (description != null)
            Description = description.Trim();
        if (notes != null)
            Notes = notes.Trim();

        QueueDomainEvent(new CheckUpdated(Id, CheckNumber, BankAccountCode));
        return this;
    }

    /// <summary>
    /// Check if the check is voided.
    /// </summary>
    /// <returns>True if voided, false otherwise.</returns>
    public bool IsVoided() => Status == "Void";

    /// <summary>
    /// Check if the check is available for use.
    /// </summary>
    /// <returns>True if available, false otherwise.</returns>
    public bool IsAvailable() => Status == "Available" && !IsVoided() && !IsStopPayment;

    /// <summary>
    /// Check if the check is cleared.
    /// </summary>
    /// <returns>True if cleared, false otherwise.</returns>
    public bool IsCleared() => Status == "Cleared";

    /// <summary>
    /// Check if the check is outstanding (issued but not cleared).
    /// </summary>
    /// <returns>True if outstanding, false otherwise.</returns>
    public bool IsOutstanding() => Status == "Issued" && !ClearedDate.HasValue;
}


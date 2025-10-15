namespace FSH.Starter.Blazor.Client.Pages.Accounting.Checks;

/// <summary>
/// ViewModel used by the Checks page for add/edit operations.
/// Represents check registration and details for display and editing.
/// </summary>
public class CheckViewModel
{
    /// <summary>
    /// Primary identifier of the check.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique check number printed on the check.
    /// Example: "1001", "CHK-2025-001". Must be unique per bank account.
    /// </summary>
    public string CheckNumber { get; set; } = string.Empty;

    /// <summary>
    /// Bank account code from which the check is drawn.
    /// Example: "102" for checking account. Links to ChartOfAccount.
    /// </summary>
    public string BankAccountCode { get; set; } = string.Empty;

    /// <summary>
    /// Bank account name for display purposes.
    /// Example: "Operating Checking Account", "Payroll Account".
    /// </summary>
    public string? BankAccountName { get; set; }

    /// <summary>
    /// Check status: Available, Issued, Cleared, Void, Stale, StopPayment.
    /// </summary>
    public string Status { get; set; } = "Available";

    /// <summary>
    /// Amount written on the check when issued.
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Payee name written on the check.
    /// Example: "ABC Suppliers Inc.", "John Doe".
    /// </summary>
    public string? PayeeName { get; set; }

    /// <summary>
    /// Optional vendor ID if check is issued to a vendor.
    /// </summary>
    public DefaultIdType? VendorId { get; set; }

    /// <summary>
    /// Optional payee ID if check is issued to a payee.
    /// </summary>
    public DefaultIdType? PayeeId { get; set; }

    /// <summary>
    /// Date when the check was issued/written.
    /// </summary>
    public DateTime? IssuedDate { get; set; }

    /// <summary>
    /// Date when the check cleared the bank.
    /// </summary>
    public DateTime? ClearedDate { get; set; }

    /// <summary>
    /// Date when the check was voided.
    /// </summary>
    public DateTime? VoidedDate { get; set; }

    /// <summary>
    /// Reason for voiding the check.
    /// </summary>
    public string? VoidReason { get; set; }

    /// <summary>
    /// Optional payment transaction ID.
    /// </summary>
    public DefaultIdType? PaymentId { get; set; }

    /// <summary>
    /// Optional expense transaction ID.
    /// </summary>
    public DefaultIdType? ExpenseId { get; set; }

    /// <summary>
    /// Memo field for check details.
    /// </summary>
    public string? Memo { get; set; }

    /// <summary>
    /// Whether the check has been printed.
    /// </summary>
    public bool IsPrinted { get; set; }

    /// <summary>
    /// Date when the check was printed.
    /// </summary>
    public DateTime? PrintedDate { get; set; }

    /// <summary>
    /// User who printed the check.
    /// </summary>
    public string? PrintedBy { get; set; }

    /// <summary>
    /// Whether a stop payment has been requested.
    /// </summary>
    public bool IsStopPayment { get; set; }

    /// <summary>
    /// Date when stop payment was requested.
    /// </summary>
    public DateTime? StopPaymentDate { get; set; }

    /// <summary>
    /// Reason for stop payment request.
    /// </summary>
    public string? StopPaymentReason { get; set; }

    /// <summary>
    /// Detailed description of the check.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes about the check.
    /// </summary>
    public string? Notes { get; set; }
}

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Checks;

/// <summary>
/// ViewModel for Check page - combines CheckUpdateCommand with display-only fields.
/// Inherits from CheckUpdateCommand to leverage Mapster mapping automatically.
/// Follows the same pattern as Catalog and Todo pages.
/// </summary>
public partial class CheckViewModel : CheckUpdateCommand
{
    /// <summary>
    /// Display-only: The unique identifier for this Check entity.
    /// Used for identification and routing.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Display-only: Name of the bank account associated with this check.
    /// Auto-populated from ChartOfAccount when account code is specified.
    /// Example: "Petty Cash Account", "Operating Account".
    /// </summary>
    public string? BankAccountName { get; set; }

    /// <summary>
    /// Display-only: Name of the bank where the account is held.
    /// Auto-populated from Bank entity when bank ID is specified.
    /// Example: "First National Bank", "Chase".
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// Start check number for bundle creation (Create mode only).
    /// Example: "3453000" to start a new check pad.
    /// Only used during Create operations for registering check bundles.
    /// </summary>
    public string? StartCheckNumber { get; set; }

    /// <summary>
    /// End check number for bundle creation (Create mode only).
    /// Example: "3453500" to end the check pad (500 checks total).
    /// Only used during Create operations for registering check bundles.
    /// </summary>
    public string? EndCheckNumber { get; set; }

    /// <summary>
    /// Display-only: Check status (Available, Issued, Cleared, Void, Stale, StopPayment).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Display-only: Amount written on the check when issued.
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Display-only: Payee name written on the check.
    /// Example: "ABC Suppliers Inc.", "John Doe".
    /// </summary>
    public string? PayeeName { get; set; }

    /// <summary>
    /// Display-only: Date when the check was issued/written.
    /// </summary>
    public DateTime? IssuedDate { get; set; }

    /// <summary>
    /// Display-only: Date when the check cleared the bank.
    /// </summary>
    public DateTime? ClearedDate { get; set; }

    /// <summary>
    /// Display-only: Date when the check was voided.
    /// </summary>
    public DateTime? VoidedDate { get; set; }

    /// <summary>
    /// Display-only: Reason for voiding the check.
    /// </summary>
    public string? VoidReason { get; set; }

    /// <summary>
    /// Display-only: Optional vendor ID if check is issued to a vendor.
    /// </summary>
    public DefaultIdType? VendorId { get; set; }

    /// <summary>
    /// Display-only: Optional payee ID if check is issued to a payee.
    /// </summary>
    public DefaultIdType? PayeeId { get; set; }

    /// <summary>
    /// Display-only: Optional payment transaction ID.
    /// </summary>
    public DefaultIdType? PaymentId { get; set; }

    /// <summary>
    /// Display-only: Optional expense transaction ID.
    /// </summary>
    public DefaultIdType? ExpenseId { get; set; }

    /// <summary>
    /// Display-only: Memo field for check details.
    /// </summary>
    public string? Memo { get; set; }

    /// <summary>
    /// Display-only: Whether the check has been printed.
    /// </summary>
    public bool IsPrinted { get; set; }

    /// <summary>
    /// Display-only: Date when the check was printed.
    /// </summary>
    public DateTime? PrintedDate { get; set; }

    /// <summary>
    /// Display-only: User who printed the check.
    /// </summary>
    public string? PrintedBy { get; set; }

    /// <summary>
    /// Display-only: Whether a stop payment has been requested.
    /// </summary>
    public bool IsStopPayment { get; set; }

    /// <summary>
    /// Display-only: Date when stop payment was requested.
    /// </summary>
    public DateTime? StopPaymentDate { get; set; }

    /// <summary>
    /// Display-only: Reason for stop payment request.
    /// </summary>
    public string? StopPaymentReason { get; set; }
}

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanRepayments;

/// <summary>
/// ViewModel used by the LoanRepayments page for add operations.
/// Mirrors the shape of the API's CreateLoanRepaymentCommand so Mapster/Adapt can map between them.
/// </summary>
public class LoanRepaymentViewModel
{
    /// <summary>
    /// Primary identifier of the loan repayment.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The loan to apply this repayment to. Required.
    /// </summary>
    public DefaultIdType LoanId { get; set; }

    /// <summary>
    /// The repayment amount.
    /// </summary>
    public decimal Amount { get; set; } = 5000m;

    /// <summary>
    /// Payment method (e.g., CASH, BANK_TRANSFER, MOBILE_MONEY).
    /// </summary>
    public string? PaymentMethod { get; set; } = "CASH";

    /// <summary>
    /// Transaction reference number for tracking.
    /// </summary>
    public string? TransactionReference { get; set; }

    /// <summary>
    /// Notes about this repayment.
    /// </summary>
    public string? Notes { get; set; }
}

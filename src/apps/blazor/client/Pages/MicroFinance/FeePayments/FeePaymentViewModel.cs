namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FeePayments;

/// <summary>
/// ViewModel for fee payment entities.
/// Represents payments made against fee charges.
/// </summary>
public class FeePaymentViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// The fee charge being paid.
    /// </summary>
    public DefaultIdType FeeChargeId { get; set; }
    
    /// <summary>
    /// Reference number for the payment.
    /// </summary>
    public string? Reference { get; set; }
    
    /// <summary>
    /// Payment amount.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Payment method (Cash, Check, Transfer, etc.).
    /// </summary>
    public string? PaymentMethod { get; set; }
    
    /// <summary>
    /// Payment source (Cash, LoanRepayment, SavingsDeduction, etc.).
    /// </summary>
    public string? PaymentSource { get; set; }
    
    /// <summary>
    /// Date of payment.
    /// </summary>
    public DateTime? PaymentDate { get; set; } = DateTime.Today;
    
    /// <summary>
    /// Optional - linked loan repayment.
    /// </summary>
    public DefaultIdType? LoanRepaymentId { get; set; }
    
    /// <summary>
    /// Optional - linked savings transaction.
    /// </summary>
    public DefaultIdType? SavingsTransactionId { get; set; }
    
    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }
}

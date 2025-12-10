namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanGuarantors;

/// <summary>
/// ViewModel for creating loan guarantor entities.
/// Maps form data to <see cref="CreateLoanGuarantorCommand"/>.
/// </summary>
public class LoanGuarantorViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// The loan being guaranteed.
    /// </summary>
    public DefaultIdType LoanId { get; set; }
    
    /// <summary>
    /// The member providing the guarantee.
    /// </summary>
    public DefaultIdType GuarantorMemberId { get; set; }
    
    /// <summary>
    /// Amount guaranteed by this guarantor.
    /// </summary>
    public decimal GuaranteedAmount { get; set; }
    
    /// <summary>
    /// Relationship between guarantor and borrower.
    /// </summary>
    public string? Relationship { get; set; }
    
    /// <summary>
    /// Date when guarantee was provided.
    /// </summary>
    public DateTime? GuaranteeDate { get; set; } = DateTime.Today;
    
    /// <summary>
    /// Date when guarantee expires.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
    
    /// <summary>
    /// Additional notes about the guarantee.
    /// </summary>
    public string? Notes { get; set; }
}

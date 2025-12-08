namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FeeCharges;

/// <summary>
/// ViewModel for fee charge entities.
/// Represents fees charged to members on their accounts.
/// </summary>
public class FeeChargeViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// The fee definition being applied.
    /// </summary>
    public Guid FeeDefinitionId { get; set; }
    
    /// <summary>
    /// The member being charged.
    /// </summary>
    public Guid MemberId { get; set; }
    
    /// <summary>
    /// Optional - the loan associated with this charge.
    /// </summary>
    public Guid? LoanId { get; set; }
    
    /// <summary>
    /// Optional - the savings account associated with this charge.
    /// </summary>
    public Guid? SavingsAccountId { get; set; }
    
    /// <summary>
    /// Optional - the share account associated with this charge.
    /// </summary>
    public Guid? ShareAccountId { get; set; }
    
    /// <summary>
    /// Reference number for the charge.
    /// </summary>
    public string? Reference { get; set; }
    
    /// <summary>
    /// Date the fee was charged.
    /// </summary>
    public DateTime? ChargeDate { get; set; } = DateTime.Today;
    
    /// <summary>
    /// Due date for payment.
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// Amount of the fee.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }
}

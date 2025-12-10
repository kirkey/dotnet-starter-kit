namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InterestRateChanges;

/// <summary>
/// ViewModel for interest rate change entities.
/// Represents loan interest rate modifications with approval workflow.
/// </summary>
public class InterestRateChangeViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// The loan whose rate is being changed.
    /// </summary>
    public DefaultIdType LoanId { get; set; }
    
    /// <summary>
    /// Reference number for the rate change.
    /// </summary>
    public string? Reference { get; set; }
    
    /// <summary>
    /// Type of rate change (Promotion, Penalty, MarketAdjustment, Restructure, etc.).
    /// </summary>
    public string? ChangeType { get; set; }
    
    /// <summary>
    /// Date of change request.
    /// </summary>
    public DateTime? RequestDate { get; set; } = DateTime.Today;
    
    /// <summary>
    /// When the new rate takes effect.
    /// </summary>
    public DateTime? EffectiveDate { get; set; } = DateTime.Today;
    
    /// <summary>
    /// Previous interest rate.
    /// </summary>
    public decimal PreviousRate { get; set; }
    
    /// <summary>
    /// New interest rate.
    /// </summary>
    public decimal NewRate { get; set; }
    
    /// <summary>
    /// Reason for the rate change.
    /// </summary>
    public string? ChangeReason { get; set; }
    
    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }
}

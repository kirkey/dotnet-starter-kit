namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FeeWaivers;

/// <summary>
/// ViewModel for fee waiver entities.
/// Represents formal fee waiver requests with approval workflow.
/// </summary>
public class FeeWaiverViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// The fee charge being waived.
    /// </summary>
    public DefaultIdType FeeChargeId { get; set; }
    
    /// <summary>
    /// Reference number for the waiver.
    /// </summary>
    public string? Reference { get; set; }
    
    /// <summary>
    /// Type of waiver (Full, Partial).
    /// </summary>
    public string? WaiverType { get; set; }
    
    /// <summary>
    /// Date of waiver request.
    /// </summary>
    public DateTime? RequestDate { get; set; } = DateTime.Today;
    
    /// <summary>
    /// Original amount of the fee.
    /// </summary>
    public decimal OriginalAmount { get; set; }
    
    /// <summary>
    /// Amount being waived.
    /// </summary>
    public decimal WaivedAmount { get; set; }
    
    /// <summary>
    /// Reason for the waiver.
    /// </summary>
    public string? WaiverReason { get; set; }
    
    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Date when the waiver was approved.
    /// </summary>
    public DateTime? ApprovalDate { get; set; }
}

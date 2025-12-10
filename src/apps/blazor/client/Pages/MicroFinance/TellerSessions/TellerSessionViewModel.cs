namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.TellerSessions;

/// <summary>
/// ViewModel for opening a teller session.
/// Maps form data to <see cref="OpenTellerSessionCommand"/>.
/// </summary>
public class TellerSessionViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Branch where the session is opened.
    /// </summary>
    public DefaultIdType BranchId { get; set; }
    
    /// <summary>
    /// Cash vault being used for the session.
    /// </summary>
    public DefaultIdType CashVaultId { get; set; }
    
    /// <summary>
    /// Session number for identification.
    /// </summary>
    public string? SessionNumber { get; set; }
    
    /// <summary>
    /// User ID of the teller.
    /// </summary>
    public DefaultIdType TellerUserId { get; set; }
    
    /// <summary>
    /// Name of the teller.
    /// </summary>
    public string? TellerName { get; set; }
    
    /// <summary>
    /// Opening cash balance.
    /// </summary>
    public decimal OpeningBalance { get; set; }
}

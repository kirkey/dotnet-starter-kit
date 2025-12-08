namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MobileWallets;

/// <summary>
/// ViewModel for creating mobile wallet entities.
/// Maps form data to <see cref="CreateMobileWalletCommand"/>.
/// </summary>
public class MobileWalletViewModel
{
    /// <summary>
    /// Primary identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// The member who owns the wallet.
    /// </summary>
    public Guid MemberId { get; set; }
    
    /// <summary>
    /// Phone number linked to the wallet.
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Mobile money provider (e.g., M-Pesa, Airtel Money).
    /// </summary>
    public string? Provider { get; set; }
    
    /// <summary>
    /// Daily transaction limit.
    /// </summary>
    public decimal DailyLimit { get; set; }
    
    /// <summary>
    /// Monthly transaction limit.
    /// </summary>
    public decimal MonthlyLimit { get; set; }
}

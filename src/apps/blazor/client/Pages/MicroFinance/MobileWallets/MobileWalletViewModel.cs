namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MobileWallets;

/// <summary>
/// ViewModel for creating/editing mobile wallet entities.
/// Maps to CreateMobileWalletCommand.
/// </summary>
public class MobileWalletViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType MemberId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public decimal DailyLimit { get; set; } = 50000m;
    public decimal MonthlyLimit { get; set; } = 500000m;
}

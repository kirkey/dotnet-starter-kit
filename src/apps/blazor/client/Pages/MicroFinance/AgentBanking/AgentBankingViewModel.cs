namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.AgentBanking;

/// <summary>
/// ViewModel for creating/editing agent banking entities.
/// Maps to CreateAgentBankingCommand and UpdateAgentBankingCommand.
/// </summary>
public class AgentBankingViewModel
{
    public DefaultIdType Id { get; set; }

    // Basic Info
    public string AgentCode { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Address { get; set; } = string.Empty;
    public string? GpsCoordinates { get; set; }
    public string? OperatingHours { get; set; }

    // Business Settings
    public decimal CommissionRate { get; set; } = 2.5m; // Default 2.5%
    public decimal DailyTransactionLimit { get; set; } = 100000m;
    public decimal MonthlyTransactionLimit { get; set; } = 3000000m;
    public DateOnly ContractStartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    // Relationships
    public DefaultIdType? BranchId { get; set; }
}

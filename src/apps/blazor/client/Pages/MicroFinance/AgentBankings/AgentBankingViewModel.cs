namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.AgentBankings;

public class AgentBankingViewModel
{
    public string? AgentCode { get; set; }
    public string? BusinessName { get; set; }
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal DailyTransactionLimit { get; set; }
    public decimal MonthlyTransactionLimit { get; set; }
    public DateTime? ContractStartDate { get; set; }
    public Guid? BranchId { get; set; }
    public string? Email { get; set; }
    public string? GpsCoordinates { get; set; }
    public string? OperatingHours { get; set; }
}

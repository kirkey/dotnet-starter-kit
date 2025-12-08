namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionStrategies;

public class CollectionStrategyViewModel
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? LoanProductId { get; set; }
    public int TriggerDaysPastDue { get; set; }
    public int? MaxDaysPastDue { get; set; }
    public decimal? MinOutstandingAmount { get; set; }
    public decimal? MaxOutstandingAmount { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string? MessageTemplate { get; set; }
    public int Priority { get; set; } = 1;
    public int? RepeatIntervalDays { get; set; }
    public int? MaxRepetitions { get; set; }
    public bool EscalateOnFailure { get; set; }
    public bool RequiresApproval { get; set; }
    public bool IsActive { get; set; } = true;
}

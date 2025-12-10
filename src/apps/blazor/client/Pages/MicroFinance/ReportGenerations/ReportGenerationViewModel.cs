namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ReportGenerations;

public class ReportGenerationViewModel
{
    public DefaultIdType ReportDefinitionId { get; set; }
    public string? Trigger { get; set; }
    public string? OutputFormat { get; set; }
    public string? Parameters { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DefaultIdType? BranchId { get; set; }
}

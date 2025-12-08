namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ReportGenerations;

public class ReportGenerationViewModel
{
    public Guid ReportDefinitionId { get; set; }
    public string? Trigger { get; set; }
    public string? OutputFormat { get; set; }
    public string? Parameters { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid? BranchId { get; set; }
}

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerCases;

public class CustomerCaseViewModel
{
    public string? CaseNumber { get; set; }
    public Guid MemberId { get; set; }
    public string? Subject { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? Channel { get; set; }
    public string? Priority { get; set; }
    public int SlaHours { get; set; } = 24;
}

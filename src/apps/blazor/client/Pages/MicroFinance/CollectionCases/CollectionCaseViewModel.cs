namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionCases;

public class CollectionCaseViewModel
{
    public Guid LoanId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? AssignedCollectorId { get; set; }
    public string Priority { get; set; } = "Normal";
    public string? Classification { get; set; }
    public DateTimeOffset OpenedDate { get; set; } = DateTimeOffset.Now;
    public string? Notes { get; set; }
}

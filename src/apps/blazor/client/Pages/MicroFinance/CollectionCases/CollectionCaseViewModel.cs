namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionCases;

public class CollectionCaseViewModel
{
    public DefaultIdType LoanId { get; set; }
    public DefaultIdType MemberId { get; set; }
    public DefaultIdType? AssignedCollectorId { get; set; }
    public string Priority { get; set; } = "Normal";
    public string? Classification { get; set; }
    public DateTimeOffset OpenedDate { get; set; } = DateTimeOffset.Now;
    public string? Notes { get; set; }
}

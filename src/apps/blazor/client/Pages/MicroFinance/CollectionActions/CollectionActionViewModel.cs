namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionActions;

public class CollectionActionViewModel
{
    public Guid CollectionCaseId { get; set; }
    public Guid LoanId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public DateTime ActionDateTime { get; set; } = DateTime.Now;
    public string? ContactMethod { get; set; }
    public string? PhoneNumberCalled { get; set; }
    public string? ContactPerson { get; set; }
    public string? Outcome { get; set; }
    public string? Description { get; set; }
    public decimal? PromisedAmount { get; set; }
    public DateTimeOffset? PromisedDate { get; set; }
    public DateTimeOffset? NextFollowUpDate { get; set; }
}

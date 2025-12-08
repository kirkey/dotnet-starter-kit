namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerSurveys;

/// <summary>
/// ViewModel for Customer Survey add/edit operations.
/// </summary>
public class CustomerSurveyViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? SurveyType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsAnonymous { get; set; }
}

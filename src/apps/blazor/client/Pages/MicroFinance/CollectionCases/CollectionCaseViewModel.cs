namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionCases;

/// <summary>
/// ViewModel for creating collection cases.
/// Maps to CreateCollectionCaseCommand.
/// </summary>
public class CollectionCaseViewModel
{
    public DefaultIdType Id { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public DefaultIdType LoanId { get; set; }
    public DefaultIdType MemberId { get; set; }
    public int DaysPastDue { get; set; } = 30;
    public decimal AmountOverdue { get; set; }
    public decimal TotalOutstanding { get; set; }
}

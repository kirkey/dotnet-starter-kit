namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LegalActions;

public class LegalActionViewModel
{
    public Guid Id { get; set; }
    public Guid CollectionCaseId { get; set; }
    public Guid LoanId { get; set; }
    public Guid MemberId { get; set; }
    public string? CaseReference { get; set; }
    public string? ActionType { get; set; }
    public string? Status { get; set; }
    public DateTime InitiatedDate { get; set; } = DateTime.Today;
    public DateTime? FiledDate { get; set; }
    public DateTime? NextHearingDate { get; set; }
    public DateTime? JudgmentDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? CourtName { get; set; }
    public string? LawyerName { get; set; }
    public decimal ClaimAmount { get; set; }
    public decimal? JudgmentAmount { get; set; }
    public decimal AmountRecovered { get; set; }
    public decimal LegalCosts { get; set; }
    public decimal CourtFees { get; set; }
    public string? JudgmentSummary { get; set; }
}

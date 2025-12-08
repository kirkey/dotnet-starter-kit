namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditScores;

public class CreditScoreViewModel
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public Guid? LoanId { get; set; }
    public string? ScoreType { get; set; }
    public string? ScoreModel { get; set; }
    public decimal Score { get; set; }
    public decimal ScoreMin { get; set; } = 300;
    public decimal ScoreMax { get; set; } = 850;
    public decimal ScorePercentile { get; set; }
    public string? Grade { get; set; }
    public decimal? ProbabilityOfDefault { get; set; }
    public decimal? ExpectedLoss { get; set; }
    public DateTime ScoredAt { get; set; } = DateTime.Now;
    public DateTime? ValidUntil { get; set; }
    public string? Source { get; set; }
    public string? Status { get; set; }
}

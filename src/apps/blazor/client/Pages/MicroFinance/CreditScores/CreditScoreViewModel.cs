using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditScores;

public class CreditScoreViewModel
{
    public DefaultIdType Id { get; set; }

    [Required]
    public DefaultIdType MemberId { get; set; }

    [Required]
    public string ScoreType { get; set; } = "Internal";

    [Required]
    [Range(0, 1000)]
    public decimal Score { get; set; }

    [Required]
    public decimal ScoreMin { get; set; } = 300;

    [Required]
    public decimal ScoreMax { get; set; } = 850;

    public string? ScoreModel { get; set; }

    public DefaultIdType? LoanId { get; set; }

    public string? Source { get; set; }

    public DefaultIdType? CreditBureauReportId { get; set; }

    [Range(0, 1)]
    public decimal? ProbabilityOfDefault { get; set; }

    public string? ScoreFactors { get; set; }

    public DateTime? ValidUntil { get; set; }

    // Read-only properties
    public string? MemberName { get; set; }
    public string? MemberNumber { get; set; }
    public string? Grade { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool IsExpired { get; set; }
}

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Get.v1;

/// <summary>
/// Response containing credit score details.
/// </summary>
public sealed record CreditScoreResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    DefaultIdType? LoanId,
    string ScoreType,
    string? ScoreModel,
    decimal Score,
    decimal ScoreMin,
    decimal ScoreMax,
    decimal ScorePercentile,
    string Grade,
    decimal? ProbabilityOfDefault,
    decimal? ExpectedLoss,
    DateTime ScoredAt,
    DateTime? ValidUntil,
    string? Source,
    string Status,
    bool IsValid);

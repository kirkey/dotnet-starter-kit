namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Create.v1;

/// <summary>
/// Response after creating a credit score.
/// </summary>
public sealed record CreateCreditScoreResponse(
    Guid Id,
    Guid MemberId,
    string ScoreType,
    decimal Score,
    string Grade,
    decimal ScorePercentile);

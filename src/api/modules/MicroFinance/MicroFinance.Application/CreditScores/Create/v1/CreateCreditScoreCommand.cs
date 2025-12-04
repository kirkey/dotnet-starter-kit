using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Create.v1;

/// <summary>
/// Command to create a new credit score record.
/// </summary>
public sealed record CreateCreditScoreCommand(
    Guid MemberId,
    string ScoreType,
    decimal Score,
    decimal ScoreMin,
    decimal ScoreMax,
    string? ScoreModel = null,
    Guid? LoanId = null,
    string? Source = null,
    Guid? CreditBureauReportId = null,
    decimal? ProbabilityOfDefault = null,
    string? ScoreFactors = null,
    DateTime? ValidUntil = null) : IRequest<CreateCreditScoreResponse>;

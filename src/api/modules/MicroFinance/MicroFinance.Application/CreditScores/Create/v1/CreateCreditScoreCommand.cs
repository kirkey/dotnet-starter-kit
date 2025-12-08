using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Create.v1;

/// <summary>
/// Command to create a new credit score record.
/// </summary>
public sealed record CreateCreditScoreCommand(
    DefaultIdType MemberId,
    string ScoreType,
    decimal Score,
    decimal ScoreMin,
    decimal ScoreMax,
    string? ScoreModel = null,
    DefaultIdType? LoanId = null,
    string? Source = null,
    DefaultIdType? CreditBureauReportId = null,
    decimal? ProbabilityOfDefault = null,
    string? ScoreFactors = null,
    DateTime? ValidUntil = null) : IRequest<CreateCreditScoreResponse>;

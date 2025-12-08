using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Get.v1;

/// <summary>
/// Specification for getting a credit score by ID.
/// </summary>
public sealed class CreditScoreByIdSpec : Specification<CreditScore, CreditScoreResponse>
{
    public CreditScoreByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);

        Query.Select(c => new CreditScoreResponse(
            c.Id,
            c.MemberId,
            c.LoanId,
            c.ScoreType,
            c.ScoreModel,
            c.Score,
            c.ScoreMin,
            c.ScoreMax,
            c.ScorePercentile,
            c.Grade,
            c.ProbabilityOfDefault,
            c.ExpectedLoss,
            c.ScoredAt,
            c.ValidUntil,
            c.Source,
            c.Status,
            c.IsValid()));
    }
}

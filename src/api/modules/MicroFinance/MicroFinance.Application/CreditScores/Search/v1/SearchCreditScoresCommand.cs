using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Search.v1;

public class SearchCreditScoresCommand : PaginationFilter, IRequest<PagedList<CreditScoreSummaryResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public string? ScoreType { get; set; }
    public string? ScoreModel { get; set; }
    public string? Grade { get; set; }
    public decimal? ScoreMin { get; set; }
    public decimal? ScoreMax { get; set; }
    public DateTimeOffset? ScoreDateFrom { get; set; }
    public DateTimeOffset? ScoreDateTo { get; set; }
}

public sealed record CreditScoreSummaryResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    DefaultIdType? LoanId,
    string ScoreType,
    string? ScoreModel,
    decimal Score,
    decimal ScoreMin,
    decimal ScoreMax,
    string Grade,
    decimal? ProbabilityOfDefault,
    DateTimeOffset CreatedOn);

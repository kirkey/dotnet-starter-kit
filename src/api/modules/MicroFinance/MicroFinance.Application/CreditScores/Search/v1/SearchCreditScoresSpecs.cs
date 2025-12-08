using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Search.v1;

public class SearchCreditScoresSpecs : EntitiesByPaginationFilterSpec<CreditScore, CreditScoreSummaryResponse>
{
    public SearchCreditScoresSpecs(SearchCreditScoresCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.ScoreType == command.ScoreType, !string.IsNullOrWhiteSpace(command.ScoreType))
            .Where(x => x.ScoreModel == command.ScoreModel, !string.IsNullOrWhiteSpace(command.ScoreModel))
            .Where(x => x.Grade == command.Grade, !string.IsNullOrWhiteSpace(command.Grade))
            .Where(x => x.Score >= command.ScoreMin!.Value, command.ScoreMin.HasValue)
            .Where(x => x.Score <= command.ScoreMax!.Value, command.ScoreMax.HasValue)
            .Where(x => x.CreatedOn >= command.ScoreDateFrom!.Value, command.ScoreDateFrom.HasValue)
            .Where(x => x.CreatedOn <= command.ScoreDateTo!.Value, command.ScoreDateTo.HasValue);
}

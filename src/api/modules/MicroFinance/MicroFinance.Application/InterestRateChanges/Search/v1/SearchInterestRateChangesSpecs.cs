using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Search.v1;

public class SearchInterestRateChangesSpecs : EntitiesByPaginationFilterSpec<InterestRateChange, InterestRateChangeSummaryResponse>
{
    public SearchInterestRateChangesSpecs(SearchInterestRateChangesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.RequestDate, !command.HasOrderBy())
            .ThenByDescending(x => x.CreatedOn)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.Reference.Contains(command.Reference!), !string.IsNullOrWhiteSpace(command.Reference))
            .Where(x => x.ChangeType == command.ChangeType, !string.IsNullOrWhiteSpace(command.ChangeType))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.RequestDate >= command.RequestDateFrom!.Value, command.RequestDateFrom.HasValue)
            .Where(x => x.RequestDate <= command.RequestDateTo!.Value, command.RequestDateTo.HasValue)
            .Where(x => x.EffectiveDate >= command.EffectiveDateFrom!.Value, command.EffectiveDateFrom.HasValue)
            .Where(x => x.EffectiveDate <= command.EffectiveDateTo!.Value, command.EffectiveDateTo.HasValue);
}

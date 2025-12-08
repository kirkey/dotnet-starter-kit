using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Search.v1;

public class SearchCollateralReleasesSpecs : EntitiesByPaginationFilterSpec<CollateralRelease, CollateralReleaseSummaryResponse>
{
    public SearchCollateralReleasesSpecs(SearchCollateralReleasesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.RequestDate, !command.HasOrderBy())
            .Where(x => x.CollateralId == command.CollateralId!.Value, command.CollateralId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.ReleaseReference.Contains(command.ReleaseReference!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.ReleaseReference))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.ReleaseMethod == command.ReleaseMethod, !string.IsNullOrWhiteSpace(command.ReleaseMethod))
            .Where(x => x.RequestDate >= command.RequestDateFrom!.Value, command.RequestDateFrom.HasValue)
            .Where(x => x.RequestDate <= command.RequestDateTo!.Value, command.RequestDateTo.HasValue)
            .Where(x => x.ReleasedDate >= command.ReleasedDateFrom!.Value, command.ReleasedDateFrom.HasValue)
            .Where(x => x.ReleasedDate <= command.ReleasedDateTo!.Value, command.ReleasedDateTo.HasValue)
            .Where(x => x.RequestedById == command.RequestedById!.Value, command.RequestedById.HasValue);
}

using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Search.v1;

public class SearchCollateralTypesSpecs : EntitiesByPaginationFilterSpec<CollateralType, CollateralTypeSummaryResponse>
{
    public SearchCollateralTypesSpecs(SearchCollateralTypesCommand command)
        : base(command) =>
        Query
            .OrderBy(x => x.DisplayOrder, !command.HasOrderBy())
            .ThenBy(x => x.Name)
            .Where(x => x.Code == command.Code, !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.Category == command.Category, !string.IsNullOrWhiteSpace(command.Category))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.RequiresInsurance == command.RequiresInsurance!.Value, command.RequiresInsurance.HasValue)
            .Where(x => x.RequiresAppraisal == command.RequiresAppraisal!.Value, command.RequiresAppraisal.HasValue)
            .Where(x => x.RequiresRegistration == command.RequiresRegistration!.Value, command.RequiresRegistration.HasValue);
}

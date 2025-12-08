using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Search.v1;

public class SearchRiskCategoriesSpecs : EntitiesByPaginationFilterSpec<RiskCategory, RiskCategorySummaryResponse>
{
    public SearchRiskCategoriesSpecs(SearchRiskCategoriesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.Code.Contains(command.Code!), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.Name.Contains(command.Name!), !string.IsNullOrWhiteSpace(command.Name))
            .Where(x => x.RiskType == command.RiskType, !string.IsNullOrWhiteSpace(command.RiskType))
            .Where(x => x.ParentCategoryId == command.ParentCategoryId!.Value, command.ParentCategoryId.HasValue)
            .Where(x => x.DefaultSeverity == command.DefaultSeverity, !string.IsNullOrWhiteSpace(command.DefaultSeverity))
            .Where(x => x.RequiresEscalation == command.RequiresEscalation!.Value, command.RequiresEscalation.HasValue);
}

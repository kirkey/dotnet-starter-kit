using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Search.v1;

/// <summary>
/// Specification for searching savings products with filters and pagination.
/// </summary>
public class SearchSavingsProductsSpecs : EntitiesByPaginationFilterSpec<SavingsProduct, SavingsProductResponse>
{
    public SearchSavingsProductsSpecs(SearchSavingsProductsCommand command)
        : base(command) =>
        Query
            .OrderBy(p => p.Name, !command.HasOrderBy())
            .Where(p => p.IsActive == command.IsActive!.Value, command.IsActive.HasValue)
            .Where(p => p.AllowOverdraft == command.AllowOverdraft!.Value, command.AllowOverdraft.HasValue)
            .Where(p => p.InterestRate >= command.MinInterestRate!.Value, command.MinInterestRate.HasValue)
            .Where(p => p.InterestRate <= command.MaxInterestRate!.Value, command.MaxInterestRate.HasValue);
}

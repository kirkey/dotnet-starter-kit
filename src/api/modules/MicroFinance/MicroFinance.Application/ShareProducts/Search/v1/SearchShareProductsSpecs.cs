using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Search.v1;

public class SearchShareProductsSpecs : EntitiesByPaginationFilterSpec<ShareProduct, ShareProductResponse>
{
    public SearchShareProductsSpecs(SearchShareProductsCommand command)
        : base(command) =>
        Query
            .OrderBy(sp => sp.Name, !command.HasOrderBy())
            .Where(sp => sp.Code == command.Code, !string.IsNullOrWhiteSpace(command.Code))
            .Where(sp => sp.Name.Contains(command.Name!), !string.IsNullOrWhiteSpace(command.Name))
            .Where(sp => sp.IsActive == command.IsActive!.Value, command.IsActive.HasValue)
            .Where(sp => sp.PaysDividends == command.PaysDividends!.Value, command.PaysDividends.HasValue)
            .Where(sp => sp.AllowTransfer == command.AllowTransfer!.Value, command.AllowTransfer.HasValue);
}

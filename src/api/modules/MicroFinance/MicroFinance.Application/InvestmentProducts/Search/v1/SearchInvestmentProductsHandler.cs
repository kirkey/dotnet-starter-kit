// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/InvestmentProducts/Search/v1/SearchInvestmentProductsHandler.cs
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Search.v1;

/// <summary>
/// Handler for searching investment products.
/// </summary>
public sealed class SearchInvestmentProductsHandler(
    [FromKeyedServices("microfinance:investmentproducts")] IReadRepository<InvestmentProduct> repository)
    : IRequestHandler<SearchInvestmentProductsCommand, PagedList<InvestmentProductResponse>>
{
    public async Task<PagedList<InvestmentProductResponse>> Handle(SearchInvestmentProductsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInvestmentProductsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InvestmentProductResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}


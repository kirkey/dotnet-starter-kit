using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Search.v1;

/// <summary>
/// Handler for searching savings products with filters and pagination.
/// </summary>
public sealed class SearchSavingsProductsHandler(
    ILogger<SearchSavingsProductsHandler> logger,
    [FromKeyedServices("microfinance:savingsproducts")] IReadRepository<SavingsProduct> repository)
    : IRequestHandler<SearchSavingsProductsCommand, PagedList<SavingsProductResponse>>
{
    public async Task<PagedList<SavingsProductResponse>> Handle(SearchSavingsProductsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSavingsProductsSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Searched savings products with {Count} results", items.Count);

        return new PagedList<SavingsProductResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

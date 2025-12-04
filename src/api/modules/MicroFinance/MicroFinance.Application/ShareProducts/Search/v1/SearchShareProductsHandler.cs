using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Search.v1;

public sealed class SearchShareProductsHandler(
    [FromKeyedServices("microfinance:shareproducts")] IReadRepository<ShareProduct> repository)
    : IRequestHandler<SearchShareProductsCommand, PagedList<ShareProductResponse>>
{
    public async Task<PagedList<ShareProductResponse>> Handle(SearchShareProductsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchShareProductsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ShareProductResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

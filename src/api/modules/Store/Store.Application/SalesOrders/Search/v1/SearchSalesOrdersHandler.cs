using FSH.Starter.WebApi.Store.Application.SalesOrders.Specs;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Search.v1;

public sealed class SearchSalesOrdersHandler(
    [FromKeyedServices("store:sales-orders")] IReadRepository<SalesOrder> repository)
    : IRequestHandler<SearchSalesOrdersCommand, PagedList<Get.v1.GetSalesOrderResponse>>
{
    public async Task<PagedList<Get.v1.GetSalesOrderResponse>> Handle(SearchSalesOrdersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSalesOrdersSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<Get.v1.GetSalesOrderResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}


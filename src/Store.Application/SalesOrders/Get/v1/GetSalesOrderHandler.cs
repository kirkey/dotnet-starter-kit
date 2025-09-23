using Store.Domain.Exceptions.SalesOrder;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Get.v1;

public sealed class GetSalesOrderHandler(
    ILogger<GetSalesOrderHandler> logger,
    [FromKeyedServices("store:sales-orders")] IRepository<SalesOrder> repository)
    : IRequestHandler<GetSalesOrderQuery, GetSalesOrderResponse>
{
    public async Task<GetSalesOrderResponse> Handle(GetSalesOrderQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new Specs.GetSalesOrderSpecification(request.Id);
        var so = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        _ = so ?? throw new SalesOrderNotFoundException(request.Id);

        return so.Adapt<GetSalesOrderResponse>();
    }
}

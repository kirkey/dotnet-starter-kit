using Accounting.Application.PurchaseOrders.Queries;
using Accounting.Application.PurchaseOrders.Responses;

namespace Accounting.Application.PurchaseOrders.Search.v1;

/// <summary>
/// Handler for searching purchase orders with filters.
/// </summary>
public sealed class SearchPurchaseOrdersHandler(
    ILogger<SearchPurchaseOrdersHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<PurchaseOrder> repository)
    : IRequestHandler<SearchPurchaseOrdersRequest, List<PurchaseOrderResponse>>
{
    public async Task<List<PurchaseOrderResponse>> Handle(SearchPurchaseOrdersRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PurchaseOrderSearchSpec(
            request.OrderNumber,
            request.VendorId,
            request.Status,
            request.IsFullyReceived);

        var orders = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} purchase orders", orders.Count);

        return orders.Select(order => new PurchaseOrderResponse
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            VendorId = order.VendorId,
            VendorName = order.VendorName,
            TotalAmount = order.TotalAmount,
            Status = order.Status.ToString(),
            IsFullyReceived = order.IsFullyReceived,
            Description = order.Description,
            Notes = order.Notes
        }).ToList();
    }
}


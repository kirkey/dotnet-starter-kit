using Accounting.Application.PurchaseOrders.Queries;
using Accounting.Application.PurchaseOrders.Responses;

namespace Accounting.Application.PurchaseOrders.Get;

public class GetPurchaseOrderHandler(
    [FromKeyedServices("accounting")] IReadRepository<PurchaseOrder> repository)
    : IRequestHandler<GetPurchaseOrderRequest, PurchaseOrderResponse>
{
    public async Task<PurchaseOrderResponse> Handle(
        GetPurchaseOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await repository.FirstOrDefaultAsync(
            new PurchaseOrderByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (order is null)
        {
            throw new NotFoundException(
                $"{nameof(PurchaseOrder)} with ID {request.Id} was not found.");
        }

        return new PurchaseOrderResponse
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
        };
    }
}


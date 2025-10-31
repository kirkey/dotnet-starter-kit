using Accounting.Application.PurchaseOrders.Queries;

namespace Accounting.Application.PurchaseOrders.Create.v1;

/// <summary>
/// Handler for creating a new purchase order.
/// </summary>
public sealed class PurchaseOrderCreateHandler(
    ILogger<PurchaseOrderCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<PurchaseOrderCreateCommand, PurchaseOrderCreateResponse>
{
    public async Task<PurchaseOrderCreateResponse> Handle(PurchaseOrderCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate order number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new PurchaseOrderByNumberSpec(request.OrderNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicatePurchaseOrderNumberException(request.OrderNumber);
        }

        var purchaseOrder = PurchaseOrder.Create(
            orderNumber: request.OrderNumber,
            orderDate: request.OrderDate,
            vendorId: request.VendorId,
            vendorName: request.VendorName,
            requesterId: request.RequesterId,
            requesterName: request.RequesterName,
            costCenterId: request.CostCenterId,
            projectId: request.ProjectId,
            expectedDeliveryDate: request.ExpectedDeliveryDate,
            shipToAddress: request.ShipToAddress,
            paymentTerms: request.PaymentTerms,
            referenceNumber: request.ReferenceNumber,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(purchaseOrder, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Purchase order created {PurchaseOrderId} - {OrderNumber}", 
            purchaseOrder.Id, purchaseOrder.OrderNumber);
        return new PurchaseOrderCreateResponse(purchaseOrder.Id);
    }
}


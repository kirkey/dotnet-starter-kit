using FSH.Framework.Core.Exceptions;
using Store.Domain.Exceptions.PurchaseOrder;
using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;

/// <summary>
/// Handler for UpdatePurchaseOrderCommand.
/// Updates PurchaseOrder entity including basic details, financial amounts, and delivery information.
/// </summary>
public sealed class UpdatePurchaseOrderHandler(
    ILogger<UpdatePurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository,
    [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> supplierRepository)
    : IRequestHandler<UpdatePurchaseOrderCommand, UpdatePurchaseOrderResponse>
{
    public async Task<UpdatePurchaseOrderResponse> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var po = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.Id);

        // Verify supplier exists and is active
        if (request.SupplierId.HasValue)
        {
            var supplier = await supplierRepository.GetByIdAsync(request.SupplierId.Value, cancellationToken).ConfigureAwait(false);
            _ = supplier ?? throw new SupplierNotFoundException(request.SupplierId.Value);
            if (!supplier.IsActive)
                throw new ConflictException($"Supplier '{supplier.Id}' is inactive and cannot accept purchase orders.");
        }

        // Update basic purchase order details
        var updated = po.Update(
            request.OrderNumber ?? po.OrderNumber,
            request.SupplierId ?? po.SupplierId,
            request.OrderDate ?? po.OrderDate,
            request.ExpectedDeliveryDate ?? po.ExpectedDeliveryDate,
            request.Status ?? po.Status,
            request.Notes ?? po.Notes,
            request.DeliveryAddress ?? po.DeliveryAddress,
            request.ContactPerson ?? po.ContactPerson,
            request.ContactPhone ?? po.ContactPhone,
            request.IsUrgent ?? po.IsUrgent);

        // Update financial amounts if provided
        if (request.TaxAmount.HasValue)
        {
            updated.UpdateTaxAmount(request.TaxAmount.Value);
        }

        if (request.DiscountAmount.HasValue)
        {
            updated.ApplyDiscount(request.DiscountAmount.Value);
        }

        // Update delivery date if provided
        if (request.ActualDeliveryDate.HasValue)
        {
            updated.UpdateDeliveryDate(request.ActualDeliveryDate.Value);
        }

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("purchase order with id : {PurchaseOrderId} updated.", po.Id);
        return new UpdatePurchaseOrderResponse(po.Id);
    }
}

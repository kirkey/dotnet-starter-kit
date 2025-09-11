using FSH.Framework.Core.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Create.v1;

public sealed record CreatePurchaseOrderCommand(
    string OrderNumber,
    DateTime OrderDate,
    DateTime ExpectedDeliveryDate,
    decimal SubTotal,
    decimal TaxAmount,
    decimal TotalAmount,
    string? Notes,
    string CreatedByName,
    DefaultIdType SupplierId,
    DefaultIdType WarehouseId
) : IRequest<CreatePurchaseOrderResponse>;


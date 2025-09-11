using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Update.v1;

public sealed record UpdatePurchaseOrderCommand(
    DefaultIdType Id,
    string? OrderNumber,
    DateTime? OrderDate,
    DateTime? ExpectedDeliveryDate,
    DateTime? ActualDeliveryDate,
    PurchaseOrderStatus? Status,
    decimal? SubTotal,
    decimal? TaxAmount,
    decimal? TotalAmount,
    string? Notes) : IRequest<UpdatePurchaseOrderResponse>;


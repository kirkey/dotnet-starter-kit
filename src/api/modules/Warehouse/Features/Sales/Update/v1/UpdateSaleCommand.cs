using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Sales.Update.v1;

public sealed record UpdateSaleCommand(
    DefaultIdType Id,
    string? SaleNumber,
    DateTime? SaleDate,
    SaleStatus? Status,
    decimal? SubTotal,
    decimal? TaxAmount,
    decimal? DiscountAmount,
    decimal? TotalAmount,
    decimal? AmountPaid,
    decimal? ChangeAmount,
    string? CustomerName,
    string? CustomerPhone,
    string? CashierName,
    string? Notes) : IRequest<UpdateSaleResponse>;


using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Sales.Create.v1;

public sealed record CreateSaleCommand(
    [property: DefaultValue("S-0001")] string SaleNumber,
    DateTime SaleDate,
    decimal SubTotal,
    decimal TaxAmount,
    decimal DiscountAmount,
    decimal TotalAmount,
    decimal AmountPaid,
    decimal ChangeAmount,
    string? CustomerName,
    string? CustomerPhone,
    string CashierName,
    string? Notes,
    DefaultIdType StoreId,
    DefaultIdType? CustomerId) : IRequest<CreateSaleResponse>;


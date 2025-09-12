namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Delete.v1;

public sealed record DeleteStockAdjustmentCommand(
    DefaultIdType Id) : IRequest;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Approve.v1;

public sealed record ApproveStockAdjustmentCommand(
    DefaultIdType Id,
    string? ApprovedBy = null) : IRequest<ApproveStockAdjustmentResponse>;

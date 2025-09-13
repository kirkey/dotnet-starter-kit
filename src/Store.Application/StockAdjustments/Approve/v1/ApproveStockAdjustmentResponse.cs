namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Approve.v1;

public sealed record ApproveStockAdjustmentResponse(
    DefaultIdType AdjustmentId,
    bool Approved);


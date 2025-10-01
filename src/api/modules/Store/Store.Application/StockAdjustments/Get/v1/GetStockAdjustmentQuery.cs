namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

public record GetStockAdjustmentQuery(DefaultIdType Id) : IRequest<GetStockAdjustmentResponse>;

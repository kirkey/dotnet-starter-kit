namespace FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1;

/// <summary>
/// Command to get a stock level by ID.
/// </summary>
public sealed record GetStockLevelCommand(
    DefaultIdType Id
) : IRequest<StockLevelResponse>;

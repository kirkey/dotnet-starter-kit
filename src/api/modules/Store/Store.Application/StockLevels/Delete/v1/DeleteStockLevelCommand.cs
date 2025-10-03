namespace FSH.Starter.WebApi.Store.Application.StockLevels.Delete.v1;

/// <summary>
/// Command to delete a stock level record.
/// </summary>
public sealed record DeleteStockLevelCommand(
    DefaultIdType Id
) : IRequest;

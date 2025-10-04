namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1;

/// <summary>
/// Response returned after updating a stock adjustment.
/// </summary>
/// <param name="Id">The identifier of the updated stock adjustment.</param>
public record UpdateStockAdjustmentResponse(DefaultIdType Id);


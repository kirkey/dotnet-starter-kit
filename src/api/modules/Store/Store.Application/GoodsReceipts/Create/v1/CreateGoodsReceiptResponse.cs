namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Create.v1;

/// <summary>
/// Response for create goods receipt operation.
/// </summary>
/// <remarks>
/// Contains the identifier of the newly created goods receipt.
/// Used to return the result of goods receipt creation operations.
/// </remarks>
public sealed record CreateGoodsReceiptResponse(DefaultIdType Id);

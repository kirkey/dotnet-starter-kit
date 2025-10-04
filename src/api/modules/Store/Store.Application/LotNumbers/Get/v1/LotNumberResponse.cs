namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1;

/// <summary>
/// Response containing lot number details.
/// </summary>
public sealed record LotNumberResponse(
    DefaultIdType Id,
    string Name,
    string? Description,
    string? Notes,
    string LotCode,
    DefaultIdType ItemId,
    DefaultIdType? SupplierId,
    DateTime? ManufactureDate,
    DateTime? ExpirationDate,
    DateTime ReceiptDate,
    int QuantityReceived,
    int QuantityRemaining,
    string Status,
    string? QualityNotes,
    DateTimeOffset CreatedOn,
    DefaultIdType CreatedBy
);

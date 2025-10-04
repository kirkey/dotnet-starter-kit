namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Create.v1;

/// <summary>
/// Command to create a new lot number.
/// </summary>
public sealed record CreateLotNumberCommand(
    [property: DefaultValue("Lot Number")] string? Name,
    [property: DefaultValue(null)] string? Description,
    [property: DefaultValue(null)] string? Notes,
    string LotCode,
    DefaultIdType ItemId,
    int QuantityReceived,
    DefaultIdType? SupplierId,
    DateTime? ManufactureDate,
    DateTime? ExpirationDate,
    DateTime? ReceiptDate,
    string? QualityNotes
) : IRequest<CreateLotNumberResponse>;

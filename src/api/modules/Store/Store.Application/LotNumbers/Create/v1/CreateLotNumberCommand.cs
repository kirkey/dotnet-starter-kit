namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Create.v1;

/// <summary>
/// Command to create a new lot number.
/// </summary>
public sealed record CreateLotNumberCommand(
    string LotCode,
    DefaultIdType ItemId,
    int QuantityReceived,
    DefaultIdType? SupplierId,
    DateTime? ManufactureDate,
    DateTime? ExpirationDate,
    DateTime? ReceiptDate,
    string? QualityNotes
) : IRequest<CreateLotNumberResponse>;

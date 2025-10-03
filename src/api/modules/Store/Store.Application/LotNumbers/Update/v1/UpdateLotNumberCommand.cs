namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Update.v1;

/// <summary>
/// Command to update a lot number.
/// </summary>
public sealed record UpdateLotNumberCommand(
    DefaultIdType Id,
    string? Status,
    string? QualityNotes
) : IRequest<UpdateLotNumberResponse>;

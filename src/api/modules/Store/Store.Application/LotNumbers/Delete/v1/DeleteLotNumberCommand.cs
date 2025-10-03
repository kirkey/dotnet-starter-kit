namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Delete.v1;

/// <summary>
/// Command to delete a lot number.
/// </summary>
public sealed record DeleteLotNumberCommand(
    DefaultIdType Id
) : IRequest;

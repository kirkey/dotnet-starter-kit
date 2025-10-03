namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1;

/// <summary>
/// Command to get a lot number by ID.
/// </summary>
public sealed record GetLotNumberCommand(
    DefaultIdType Id
) : IRequest<LotNumberResponse>;

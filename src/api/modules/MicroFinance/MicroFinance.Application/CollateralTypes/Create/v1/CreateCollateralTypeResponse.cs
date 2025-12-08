namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Create.v1;

/// <summary>
/// Response after creating a collateral type.
/// </summary>
public sealed record CreateCollateralTypeResponse(
    DefaultIdType Id,
    string Name,
    string Code,
    string Category,
    string Status);

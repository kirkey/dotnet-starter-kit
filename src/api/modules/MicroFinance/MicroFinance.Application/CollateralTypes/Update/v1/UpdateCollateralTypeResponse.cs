namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Update.v1;

/// <summary>
/// Response returned after successfully updating a collateral type.
/// </summary>
/// <param name="Id">The unique identifier of the updated collateral type.</param>
public sealed record UpdateCollateralTypeResponse(DefaultIdType Id);

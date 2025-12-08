namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Cancel.v1;

/// <summary>
/// Response after cancelling a collateral release.
/// </summary>
/// <param name="Id">The unique identifier of the cancelled release.</param>
/// <param name="Status">The new status of the release.</param>
public sealed record CancelCollateralReleaseResponse(
    DefaultIdType Id,
    string Status);

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Reject.v1;

/// <summary>
/// Response after rejecting a collateral release.
/// </summary>
/// <param name="Id">The unique identifier of the rejected release.</param>
/// <param name="Status">The new status of the release.</param>
/// <param name="Reason">The rejection reason.</param>
public sealed record RejectCollateralReleaseResponse(
    DefaultIdType Id,
    string Status,
    string Reason);

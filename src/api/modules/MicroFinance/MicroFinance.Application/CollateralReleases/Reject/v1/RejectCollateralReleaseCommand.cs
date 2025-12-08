using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Reject.v1;

/// <summary>
/// Command to reject a collateral release request.
/// </summary>
/// <param name="Id">The unique identifier of the collateral release to reject.</param>
/// <param name="Reason">The reason for rejection.</param>
/// <param name="RejectedById">The ID of the user rejecting the release.</param>
public sealed record RejectCollateralReleaseCommand(
    DefaultIdType Id,
    string Reason,
    DefaultIdType RejectedById) : IRequest<RejectCollateralReleaseResponse>;

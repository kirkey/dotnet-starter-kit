using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Cancel.v1;

/// <summary>
/// Command to cancel a collateral release request.
/// </summary>
/// <param name="Id">The unique identifier of the collateral release to cancel.</param>
/// <param name="Reason">The reason for cancellation.</param>
public sealed record CancelCollateralReleaseCommand(
    DefaultIdType Id,
    string Reason) : IRequest<CancelCollateralReleaseResponse>;

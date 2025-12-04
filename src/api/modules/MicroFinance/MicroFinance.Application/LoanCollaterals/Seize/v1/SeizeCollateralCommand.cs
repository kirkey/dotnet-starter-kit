using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Seize.v1;

/// <summary>
/// Command to seize a collateral due to loan default.
/// </summary>
public sealed record SeizeCollateralCommand(Guid Id, string? Reason = null) : IRequest<SeizeCollateralResponse>;

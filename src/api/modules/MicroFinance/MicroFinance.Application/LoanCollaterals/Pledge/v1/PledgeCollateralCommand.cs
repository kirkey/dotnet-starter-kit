using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Pledge.v1;

/// <summary>
/// Command to pledge a collateral against a loan.
/// </summary>
public sealed record PledgeCollateralCommand(Guid Id) : IRequest<PledgeCollateralResponse>;

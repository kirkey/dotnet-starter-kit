using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Verify.v1;

/// <summary>
/// Command to verify a loan collateral.
/// </summary>
public sealed record VerifyCollateralCommand(Guid Id) : IRequest<VerifyCollateralResponse>;

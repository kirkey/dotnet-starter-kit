using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Apply.v1;

/// <summary>
/// Command to apply an approved interest rate change to the loan.
/// </summary>
public sealed record ApplyInterestRateChangeCommand(DefaultIdType Id) : IRequest<ApplyInterestRateChangeResponse>;

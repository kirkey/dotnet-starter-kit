using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Cancel.v1;

/// <summary>
/// Command to cancel an interest rate change.
/// </summary>
public sealed record CancelInterestRateChangeCommand(DefaultIdType Id) : IRequest<CancelInterestRateChangeResponse>;

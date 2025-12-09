using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Get.v1;

/// <summary>
/// Request to get an interest rate change by ID.
/// </summary>
public sealed record GetInterestRateChangeRequest(DefaultIdType Id) : IRequest<InterestRateChangeResponse>;

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Get.v1;

/// <summary>
/// Request to get a fee waiver by ID.
/// </summary>
public sealed record GetFeeWaiverRequest(DefaultIdType Id) : IRequest<FeeWaiverResponse>;

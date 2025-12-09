using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Cancel.v1;

/// <summary>
/// Command to cancel a pending fee waiver.
/// </summary>
public sealed record CancelFeeWaiverCommand(DefaultIdType Id) : IRequest<CancelFeeWaiverResponse>;

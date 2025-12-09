using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Reject.v1;

/// <summary>
/// Command to reject a fee waiver.
/// </summary>
public sealed record RejectFeeWaiverCommand(
    DefaultIdType Id,
    DefaultIdType RejectedByUserId,
    string RejectorName,
    string Reason) : IRequest<RejectFeeWaiverResponse>;

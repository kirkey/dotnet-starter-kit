using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Approve.v1;

/// <summary>
/// Command to approve a fee waiver.
/// </summary>
public sealed record ApproveFeeWaiverCommand(
    DefaultIdType Id,
    DefaultIdType ApprovedByUserId,
    string ApproverName) : IRequest<ApproveFeeWaiverResponse>;

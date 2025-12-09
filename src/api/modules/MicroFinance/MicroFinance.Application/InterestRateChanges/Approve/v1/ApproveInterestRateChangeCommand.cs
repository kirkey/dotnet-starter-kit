using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Approve.v1;

/// <summary>
/// Command to approve an interest rate change.
/// </summary>
public sealed record ApproveInterestRateChangeCommand(
    DefaultIdType Id,
    DefaultIdType ApprovedByUserId,
    string ApproverName) : IRequest<ApproveInterestRateChangeResponse>;

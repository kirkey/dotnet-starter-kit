using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Reject.v1;

/// <summary>
/// Command to reject an interest rate change.
/// </summary>
public sealed record RejectInterestRateChangeCommand(
    DefaultIdType Id,
    DefaultIdType RejectedByUserId,
    string RejectorName,
    string Reason) : IRequest<RejectInterestRateChangeResponse>;

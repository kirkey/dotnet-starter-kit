using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Get.v1;

/// <summary>
/// Specification for getting an interest rate change by ID.
/// </summary>
public sealed class InterestRateChangeByIdSpec : Specification<InterestRateChange, InterestRateChangeResponse>
{
    public InterestRateChangeByIdSpec(DefaultIdType id)
    {
        Query.Where(irc => irc.Id == id);

        Query.Select(irc => new InterestRateChangeResponse(
            irc.Id,
            irc.LoanId,
            irc.Reference,
            irc.ChangeType,
            irc.RequestDate,
            irc.EffectiveDate,
            irc.PreviousRate,
            irc.NewRate,
            irc.RateChange,
            irc.ChangeReason,
            irc.Status,
            irc.ApprovedByUserId,
            irc.ApprovedBy,
            irc.ApprovalDate,
            irc.AppliedDate,
            irc.RejectionReason,
            irc.Notes));
    }
}

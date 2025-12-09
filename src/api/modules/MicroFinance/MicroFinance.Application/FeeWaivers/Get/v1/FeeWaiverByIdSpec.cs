using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Get.v1;

/// <summary>
/// Specification for getting a fee waiver by ID.
/// </summary>
public sealed class FeeWaiverByIdSpec : Specification<FeeWaiver, FeeWaiverResponse>
{
    public FeeWaiverByIdSpec(DefaultIdType id)
    {
        Query.Where(fw => fw.Id == id);

        Query.Select(fw => new FeeWaiverResponse(
            fw.Id,
            fw.FeeChargeId,
            fw.Reference,
            fw.WaiverType,
            fw.RequestDate,
            fw.OriginalAmount,
            fw.WaivedAmount,
            fw.RemainingAmount,
            fw.WaiverReason,
            fw.Status,
            fw.ApprovedByUserId,
            fw.ApprovedBy,
            fw.ApprovalDate,
            fw.RejectionReason,
            fw.Notes));
    }
}

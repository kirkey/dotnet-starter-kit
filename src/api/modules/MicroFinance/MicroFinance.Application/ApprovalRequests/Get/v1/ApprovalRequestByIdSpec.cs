using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;

/// <summary>
/// Specification to get an approval request by ID.
/// </summary>
public sealed class ApprovalRequestByIdSpec : Specification<ApprovalRequest>, ISingleResultSpecification<ApprovalRequest>
{
    public ApprovalRequestByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

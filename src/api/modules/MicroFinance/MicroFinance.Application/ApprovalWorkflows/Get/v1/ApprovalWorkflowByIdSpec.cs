using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Get.v1;

/// <summary>
/// Specification to get an approval workflow by ID.
/// </summary>
public sealed class ApprovalWorkflowByIdSpec : Specification<ApprovalWorkflow>, ISingleResultSpecification<ApprovalWorkflow>
{
    public ApprovalWorkflowByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}

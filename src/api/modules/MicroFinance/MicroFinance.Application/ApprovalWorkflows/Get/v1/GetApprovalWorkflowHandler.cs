using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Get.v1;

/// <summary>
/// Handler to get an approval workflow by ID.
/// </summary>
public sealed class GetApprovalWorkflowHandler(
    ILogger<GetApprovalWorkflowHandler> logger,
    [FromKeyedServices("microfinance:approvalworkflows")] IReadRepository<ApprovalWorkflow> repository)
    : IRequestHandler<GetApprovalWorkflowRequest, ApprovalWorkflowResponse>
{
    public async Task<ApprovalWorkflowResponse> Handle(GetApprovalWorkflowRequest request, CancellationToken cancellationToken)
    {
        var workflow = await repository.FirstOrDefaultAsync(new ApprovalWorkflowByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Approval workflow {request.Id} not found");

        logger.LogInformation("Retrieved approval workflow {Id}", workflow.Id);

        return new ApprovalWorkflowResponse(
            workflow.Id,
            workflow.Code,
            workflow.Name,
            workflow.Description,
            workflow.EntityType,
            workflow.MinAmount,
            workflow.MaxAmount,
            workflow.BranchId,
            workflow.NumberOfLevels,
            workflow.IsSequential,
            workflow.IsActive,
            workflow.Priority);
    }
}

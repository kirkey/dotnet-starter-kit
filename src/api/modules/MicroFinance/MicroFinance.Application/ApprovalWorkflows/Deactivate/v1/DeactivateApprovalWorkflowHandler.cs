using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Deactivate.v1;

/// <summary>
/// Handler to deactivate an approval workflow.
/// </summary>
public sealed class DeactivateApprovalWorkflowHandler(
    ILogger<DeactivateApprovalWorkflowHandler> logger,
    [FromKeyedServices("microfinance:approvalworkflows")] IRepository<ApprovalWorkflow> repository)
    : IRequestHandler<DeactivateApprovalWorkflowCommand, DeactivateApprovalWorkflowResponse>
{
    public async Task<DeactivateApprovalWorkflowResponse> Handle(DeactivateApprovalWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = await repository.FirstOrDefaultAsync(new ApprovalWorkflowByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Approval workflow {request.Id} not found");

        workflow.Deactivate();
        await repository.UpdateAsync(workflow, cancellationToken);

        logger.LogInformation("Approval workflow {Id} deactivated", workflow.Id);

        return new DeactivateApprovalWorkflowResponse(workflow.Id, workflow.IsActive);
    }
}

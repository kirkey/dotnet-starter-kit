using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Activate.v1;

/// <summary>
/// Handler to activate an approval workflow.
/// </summary>
public sealed class ActivateApprovalWorkflowHandler(
    ILogger<ActivateApprovalWorkflowHandler> logger,
    [FromKeyedServices("microfinance:approvalworkflows")] IRepository<ApprovalWorkflow> repository)
    : IRequestHandler<ActivateApprovalWorkflowCommand, ActivateApprovalWorkflowResponse>
{
    public async Task<ActivateApprovalWorkflowResponse> Handle(ActivateApprovalWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = await repository.FirstOrDefaultAsync(new ApprovalWorkflowByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Approval workflow {request.Id} not found");

        workflow.Activate();
        await repository.UpdateAsync(workflow, cancellationToken);

        logger.LogInformation("Approval workflow {Id} activated", workflow.Id);

        return new ActivateApprovalWorkflowResponse(workflow.Id, workflow.IsActive);
    }
}

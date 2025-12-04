using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Create.v1;

/// <summary>
/// Handler for creating a new approval workflow.
/// </summary>
public sealed class CreateApprovalWorkflowHandler(
    ILogger<CreateApprovalWorkflowHandler> logger,
    [FromKeyedServices("microfinance:approvalworkflows")] IRepository<ApprovalWorkflow> repository)
    : IRequestHandler<CreateApprovalWorkflowCommand, CreateApprovalWorkflowResponse>
{
    public async Task<CreateApprovalWorkflowResponse> Handle(CreateApprovalWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = ApprovalWorkflow.Create(
            request.Code,
            request.Name,
            request.EntityType,
            request.NumberOfLevels);

        if (request.Description is not null)
        {
            workflow.Update(description: request.Description, isSequential: request.IsSequential, priority: request.Priority);
        }

        if (request.MinAmount.HasValue || request.MaxAmount.HasValue)
        {
            workflow.WithAmountThresholds(request.MinAmount, request.MaxAmount);
        }

        if (request.BranchId.HasValue)
        {
            workflow.ForBranch(request.BranchId.Value);
        }

        await repository.AddAsync(workflow, cancellationToken);
        logger.LogInformation("Approval workflow {Code} created with ID {Id}", workflow.Code, workflow.Id);

        return new CreateApprovalWorkflowResponse(workflow.Id, workflow.Code, workflow.Name, workflow.EntityType);
    }
}

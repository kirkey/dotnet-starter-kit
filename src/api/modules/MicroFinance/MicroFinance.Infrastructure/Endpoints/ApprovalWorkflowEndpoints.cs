using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class ApprovalWorkflowEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/approval-workflows").WithTags("Approval Workflows");

        group.MapPost("/", async (CreateApprovalWorkflowCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/approval-workflows/{result.Id}", result);
        })
        .WithName("CreateApprovalWorkflow")
        .WithSummary("Create a new approval workflow")
        .Produces<CreateApprovalWorkflowResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetApprovalWorkflowRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetApprovalWorkflow")
        .WithSummary("Get approval workflow by ID")
        .Produces<ApprovalWorkflowResponse>();

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateApprovalWorkflowCommand(id));
            return Results.Ok(result);
        })
        .WithName("ActivateApprovalWorkflow")
        .WithSummary("Activate approval workflow")
        .Produces<ActivateApprovalWorkflowResponse>();

        group.MapPost("/{id:guid}/deactivate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateApprovalWorkflowCommand(id));
            return Results.Ok(result);
        })
        .WithName("DeactivateApprovalWorkflow")
        .WithSummary("Deactivate approval workflow")
        .Produces<DeactivateApprovalWorkflowResponse>();

    }
}

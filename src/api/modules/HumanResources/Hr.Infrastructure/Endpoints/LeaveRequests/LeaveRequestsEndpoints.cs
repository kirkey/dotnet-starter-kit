using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Approve.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Cancel.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Reject.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Submit.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests;

/// <summary>
/// Endpoint routes for managing leave requests.
/// Supports the full leave request workflow including creation, submission, approval, and rejection.
/// </summary>
public class LeaveRequestsEndpoints() : CarterModule("humanresources")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/leave-requests").WithTags("leave-requests");

        group.MapPost("/", async (CreateLeaveRequestCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/hr/leave-requests/{response.Id}", response);
            })
            .WithName("CreateLeaveRequestEndpoint")
            .WithSummary("Creates a new leave request")
            .WithDescription("Creates a new leave request for an employee. The request is created in Draft status and must be submitted for approval. Validates employee leave balance and leave type eligibility per Philippines Labor Code.")
            .Produces<CreateLeaveRequestResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetLeaveRequestRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLeaveRequestEndpoint")
            .WithSummary("Gets a leave request by ID")
            .WithDescription("Retrieves detailed information about a specific leave request including status, dates, approval details, and attachments")
            .Produces<LeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateLeaveRequestCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateLeaveRequestEndpoint")
            .WithSummary("Updates a leave request")
            .WithDescription("Updates a leave request including status and approver comments. Primarily used for administrative updates.")
            .Produces<UpdateLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new DeleteLeaveRequestCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteLeaveRequestEndpoint")
            .WithSummary("Deletes a leave request")
            .WithDescription("Deletes a leave request. Only Draft and Rejected requests can be deleted. Approved or Submitted requests cannot be deleted without proper workflow steps.")
            .Produces<DeleteLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchLeaveRequestsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchLeaveRequestsEndpoint")
            .WithSummary("Searches leave requests")
            .WithDescription("Searches and filters leave requests by employee, leave type, status, and date range with pagination support. Supports advanced filtering per Philippines Labor Code compliance requirements.")
            .Produces<PagedList<LeaveRequestResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/{id}/submit", async (DefaultIdType id, SubmitLeaveRequestCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Accepted($"/hr/leave-requests/{response.Id}", response);
            })
            .WithName("SubmitLeaveRequestEndpoint")
            .WithSummary("Submits a leave request for approval")
            .WithDescription("Submits a Draft leave request for manager approval. Validates leave balance and eligibility per Philippines Labor Code. Request transitions to Submitted status.")
            .Produces<SubmitLeaveRequestResponse>(StatusCodes.Status202Accepted)
            .RequirePermission(FshPermission.NameFor(FshActions.Submit, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveLeaveRequestCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ApproveLeaveRequestEndpoint")
            .WithSummary("Approves a leave request")
            .WithDescription("Approves a Submitted leave request. Manager can include optional comments. Request transitions to Approved status and leave balance is updated per Philippines Labor Code.")
            .Produces<ApproveLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/{id}/reject", async (DefaultIdType id, RejectLeaveRequestCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("RejectLeaveRequestEndpoint")
            .WithSummary("Rejects a leave request")
            .WithDescription("Rejects a Submitted leave request with a required reason. Request transitions to Rejected status and reserved pending balance is released per Philippines Labor Code.")
            .Produces<RejectLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Leaves))
            .MapToApiVersion(1);

        group.MapPost("/{id}/cancel", async (DefaultIdType id, CancelLeaveRequestCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CancelLeaveRequestEndpoint")
            .WithSummary("Cancels a leave request")
            .WithDescription("Cancels a Draft or Submitted leave request by the employee. Approved requests cannot be cancelled. Reserved pending balance is released per Philippines Labor Code.")
            .Produces<CancelLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Cancel, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}


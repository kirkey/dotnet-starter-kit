using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Cancel.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Reject.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class ApprovalRequestEndpoints() : CarterModule("microfinance")
{

    private const string ApproveRequestLevel = "ApproveRequestLevel";
    private const string CancelApprovalRequest = "CancelApprovalRequest";
    private const string CreateApprovalRequest = "CreateApprovalRequest";
    private const string GetApprovalRequest = "GetApprovalRequest";
    private const string RejectApprovalRequest = "RejectApprovalRequest";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/approval-requests").WithTags("Approval Requests");

        group.MapPost("/", async (CreateApprovalRequestCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/approval-requests/{result.Id}", result);
        })
        .WithName(CreateApprovalRequest)
        .WithSummary("Create a new approval request")
        .Produces<CreateApprovalRequestResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetApprovalRequestRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetApprovalRequest)
        .WithSummary("Get approval request by ID")
        .Produces<ApprovalRequestResponse>();

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveRequestBody body, ISender sender) =>
        {
            var command = new ApproveRequestLevelCommand(id, body.ApproverId, body.Comments);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(ApproveRequestLevel)
        .WithSummary("Approve the current level of an approval request")
        .Produces<ApproveRequestLevelResponse>();

        group.MapPost("/{id:guid}/reject", async (Guid id, RejectRequestBody body, ISender sender) =>
        {
            var command = new RejectApprovalRequestCommand(id, body.ApproverId, body.Reason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(RejectApprovalRequest)
        .WithSummary("Reject an approval request")
        .Produces<RejectApprovalRequestResponse>();

        group.MapPost("/{id:guid}/cancel", async (Guid id, CancelRequestBody body, ISender sender) =>
        {
            var command = new CancelApprovalRequestCommand(id, body.Reason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(CancelApprovalRequest)
        .WithSummary("Cancel an approval request")
        .Produces<CancelApprovalRequestResponse>();

    }
}

public sealed record ApproveRequestBody(Guid ApproverId, string? Comments);
public sealed record RejectRequestBody(Guid ApproverId, string Reason);
public sealed record CancelRequestBody(string Reason);

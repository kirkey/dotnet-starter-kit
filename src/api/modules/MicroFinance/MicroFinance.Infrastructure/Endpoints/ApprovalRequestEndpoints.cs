using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Cancel.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class ApprovalRequestEndpoints : CarterModule
{

    private const string ApproveRequestLevel = "ApproveRequestLevel";
    private const string CancelApprovalRequest = "CancelApprovalRequest";
    private const string CreateApprovalRequest = "CreateApprovalRequest";
    private const string GetApprovalRequest = "GetApprovalRequest";
    private const string RejectApprovalRequest = "RejectApprovalRequest";
    private const string SearchApprovalRequests = "SearchApprovalRequests";

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
        .Produces<CreateApprovalRequestResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetApprovalRequestRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetApprovalRequest)
        .WithSummary("Get approval request by ID")
        .Produces<ApprovalRequestResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveRequestBody body, ISender sender) =>
        {
            var command = new ApproveRequestLevelCommand(id, body.ApproverId, body.Comments);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(ApproveRequestLevel)
        .WithSummary("Approve the current level of an approval request")
        .Produces<ApproveRequestLevelResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectRequestBody body, ISender sender) =>
        {
            var command = new RejectApprovalRequestCommand(id, body.ApproverId, body.Reason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(RejectApprovalRequest)
        .WithSummary("Reject an approval request")
        .Produces<RejectApprovalRequestResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelRequestBody body, ISender sender) =>
        {
            var command = new CancelApprovalRequestCommand(id, body.Reason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(CancelApprovalRequest)
        .WithSummary("Cancel an approval request")
        .Produces<CancelApprovalRequestResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Cancel, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchApprovalRequestsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchApprovalRequests)
        .WithSummary("Search approval requests")
        .Produces<PagedList<ApprovalRequestSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record ApproveRequestBody(DefaultIdType ApproverId, string? Comments);
public sealed record RejectRequestBody(DefaultIdType ApproverId, string Reason);
public sealed record CancelRequestBody(string Reason);

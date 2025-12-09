using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Cancel.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class FeeWaiverEndpoints : CarterModule
{
    private const string CreateFeeWaiver = "CreateFeeWaiver";
    private const string GetFeeWaiver = "GetFeeWaiver";
    private const string SearchFeeWaivers = "SearchFeeWaivers";
    private const string UpdateFeeWaiver = "UpdateFeeWaiver";
    private const string ApproveFeeWaiver = "ApproveFeeWaiver";
    private const string RejectFeeWaiver = "RejectFeeWaiver";
    private const string CancelFeeWaiver = "CancelFeeWaiver";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/fee-waivers").WithTags("Fee Waivers");

        group.MapPost("/", async (CreateFeeWaiverCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/fee-waivers/{result.Id}", result);
        })
        .WithName(CreateFeeWaiver)
        .WithSummary("Create a new fee waiver request")
        .Produces<CreateFeeWaiverResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetFeeWaiverRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetFeeWaiver)
        .WithSummary("Get fee waiver by ID")
        .Produces<FeeWaiverResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateFeeWaiverCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateFeeWaiver)
        .WithSummary("Update a pending fee waiver")
        .Produces<UpdateFeeWaiverResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveFeeWaiverCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ApproveFeeWaiver)
        .WithSummary("Approve a fee waiver request")
        .Produces<ApproveFeeWaiverResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectFeeWaiverCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(RejectFeeWaiver)
        .WithSummary("Reject a fee waiver request")
        .Produces<RejectFeeWaiverResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelFeeWaiverCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(CancelFeeWaiver)
        .WithSummary("Cancel a pending fee waiver")
        .Produces<CancelFeeWaiverResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchFeeWaiversCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchFeeWaivers)
        .WithSummary("Search fee waivers")
        .Produces<PagedList<FeeWaiverSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);
    }
}

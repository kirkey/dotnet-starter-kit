using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Apply.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Cancel.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InterestRateChangeEndpoints : CarterModule
{
    private const string CreateInterestRateChange = "CreateInterestRateChange";
    private const string GetInterestRateChange = "GetInterestRateChange";
    private const string SearchInterestRateChanges = "SearchInterestRateChanges";
    private const string UpdateInterestRateChange = "UpdateInterestRateChange";
    private const string ApproveInterestRateChange = "ApproveInterestRateChange";
    private const string RejectInterestRateChange = "RejectInterestRateChange";
    private const string ApplyInterestRateChange = "ApplyInterestRateChange";
    private const string CancelInterestRateChange = "CancelInterestRateChange";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/interest-rate-changes").WithTags("Interest Rate Changes");

        group.MapPost("/", async (CreateInterestRateChangeCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/interest-rate-changes/{result.Id}", result);
        })
        .WithName(CreateInterestRateChange)
        .WithSummary("Create a new interest rate change request")
        .Produces<CreateInterestRateChangeResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetInterestRateChangeRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetInterestRateChange)
        .WithSummary("Get interest rate change by ID")
        .Produces<InterestRateChangeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateInterestRateChangeCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateInterestRateChange)
        .WithSummary("Update a pending interest rate change")
        .Produces<UpdateInterestRateChangeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveInterestRateChangeCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ApproveInterestRateChange)
        .WithSummary("Approve an interest rate change request")
        .Produces<ApproveInterestRateChangeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectInterestRateChangeCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(RejectInterestRateChange)
        .WithSummary("Reject an interest rate change request")
        .Produces<RejectInterestRateChangeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/apply", async (DefaultIdType id, ApplyInterestRateChangeCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ApplyInterestRateChange)
        .WithSummary("Apply an approved interest rate change to the loan")
        .Produces<ApplyInterestRateChangeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelInterestRateChangeCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(CancelInterestRateChange)
        .WithSummary("Cancel an interest rate change request")
        .Produces<CancelInterestRateChangeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchInterestRateChangesCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchInterestRateChanges)
        .WithSummary("Search interest rate changes")
        .Produces<PagedList<InterestRateChangeSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);
    }
}

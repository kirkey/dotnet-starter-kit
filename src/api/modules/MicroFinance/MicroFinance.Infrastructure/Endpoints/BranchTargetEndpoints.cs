using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.RecordProgress.v1;
using FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class BranchTargetEndpoints : CarterModule
{

    private const string CreateBranchTarget = "CreateBranchTarget";
    private const string GetBranchTarget = "GetBranchTarget";
    private const string RecordBranchProgress = "RecordBranchProgress";
    private const string SearchBranchTargets = "SearchBranchTargets";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/branch-targets").WithTags("Branch Targets");

        group.MapPost("/", async (CreateBranchTargetCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/branch-targets/{result.Id}", result);
        })
        .WithName(CreateBranchTarget)
        .WithSummary("Create a new branch target")
        .Produces<CreateBranchTargetResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetBranchTargetRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetBranchTarget)
        .WithSummary("Get branch target by ID")
        .Produces<BranchTargetResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/progress", async (DefaultIdType id, RecordBranchProgressRequest request, ISender sender) =>
        {
            var command = new RecordBranchProgressCommand(id, request.AchievedValue);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(RecordBranchProgress)
        .WithSummary("Record progress towards branch target")
        .Produces<RecordBranchProgressResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchBranchTargetsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchBranchTargets)
        .WithSummary("Search branch targets")
        .Produces<PagedList<BranchTargetSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record RecordBranchProgressRequest(decimal AchievedValue);

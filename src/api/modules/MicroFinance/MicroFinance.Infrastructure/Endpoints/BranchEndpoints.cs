using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.AssignManager.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Dashboard;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class BranchEndpoints : CarterModule
{

    private const string ActivateBranch = "ActivateBranch";
    private const string AssignBranchManager = "AssignBranchManager";
    private const string CloseBranch = "CloseBranch";
    private const string CreateBranch = "CreateBranch";
    private const string DeactivateBranch = "DeactivateBranch";
    private const string GetBranch = "GetBranch";
    private const string GetBranchDashboard = "GetBranchDashboard";
    private const string SearchBranches = "SearchBranches";
    private const string UpdateBranch = "UpdateBranch";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/branches").WithTags("Branches");

        // CRUD Operations
        group.MapPost("/", async (CreateBranchCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/branches/{response.Id}", response);
            })
            .WithName(CreateBranch)
            .WithSummary("Creates a new branch")
            .Produces<CreateBranchResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetBranchRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetBranch)
            .WithSummary("Gets a branch by ID")
            .Produces<BranchResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateBranchCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdateBranch)
            .WithSummary("Updates a branch")
            .Produces<UpdateBranchResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/search", async ([FromBody] SearchBranchesCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchBranches)
            .WithSummary("Searches branches with filters")
            .Produces<PagedList<BranchSummaryResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Workflow Operations
        group.MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateBranchCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivateBranch)
            .WithSummary("Activates a branch")
            .Produces<ActivateBranchResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new DeactivateBranchCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DeactivateBranch)
            .WithSummary("Deactivates a branch")
            .Produces<DeactivateBranchResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/close", async (DefaultIdType id, CloseBranchCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CloseBranch)
            .WithSummary("Permanently closes a branch")
            .Produces<CloseBranchResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/assign-manager", async (DefaultIdType id, AssignBranchManagerCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(AssignBranchManager)
            .WithSummary("Assigns a manager to the branch")
            .Produces<AssignBranchManagerResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Dashboard
        group.MapGet("/{id:guid}/dashboard", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetBranchDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetBranchDashboard)
            .WithSummary("Gets comprehensive branch dashboard analytics")
            .Produces<BranchDashboardResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}

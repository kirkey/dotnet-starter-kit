using FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;
using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Assign.v1;
using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Complete.v1;
using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Create.v1;
using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Delete.v1;
using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;
using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;
using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Start.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PutAwayTasks;

public class PutAwayTasksEndpoints() : CarterModule("store")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/put-away-tasks").WithTags("put-away-tasks");

        // Create a new put-away task
        group.MapPost("/", async (CreatePutAwayTaskCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreatePutAwayTaskEndpoint")
            .WithSummary("Create a new put-away task")
            .WithDescription("Creates a new put-away task for warehouse operations.")
            .Produces<CreatePutAwayTaskResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Warehouse))
            .MapToApiVersion(1);

        // Add an item to a put-away task
        group.MapPost("/{id}/items", async (DefaultIdType id, AddPutAwayTaskItemCommand request, ISender sender) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    return Results.BadRequest("Put-away task ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("AddPutAwayTaskItemEndpoint")
            .WithSummary("Add an item to a put-away task")
            .WithDescription("Adds an item to an existing put-away task for warehouse operations.")
            .Produces<AddPutAwayTaskItemResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);

        // Assign a put-away task to a worker
        group.MapPost("/{id}/assign", async (DefaultIdType id, AssignPutAwayTaskCommand request, ISender sender) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    return Results.BadRequest("Put-away task ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("AssignPutAwayTaskEndpoint")
            .WithSummary("Assign a put-away task to a worker")
            .WithDescription("Assigns a put-away task to a warehouse worker for execution.")
            .Produces<AssignPutAwayTaskResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
            .MapToApiVersion(1);

        // Start a put-away task
        group.MapPost("/{id}/start", async (DefaultIdType id, StartPutAwayCommand request, ISender sender) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    return Results.BadRequest("Put-away task ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("StartPutAwayEndpoint")
            .WithSummary("Start a put-away task")
            .WithDescription("Marks a put-away task as started and records the start time.")
            .Produces<StartPutAwayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);

        // Complete a put-away task
        group.MapPost("/{id}/complete", async (DefaultIdType id, CompletePutAwayCommand request, ISender sender) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    return Results.BadRequest("Put-away task ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CompletePutAwayEndpoint")
            .WithSummary("Complete a put-away task")
            .WithDescription("Marks a put-away task as completed and records the completion time.")
            .Produces<CompletePutAwayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);

        // Delete a put-away task
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                await sender.Send(new DeletePutAwayTaskCommand { PutAwayTaskId = id }).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeletePutAwayTaskEndpoint")
            .WithSummary("Delete a put-away task")
            .WithDescription("Deletes a put-away task and all associated items.")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Warehouse))
            .MapToApiVersion(1);

        // Get a put-away task by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetPutAwayTaskQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPutAwayTaskEndpoint")
            .WithSummary("Get a put-away task by ID")
            .WithDescription("Retrieves a specific put-away task with all items and details.")
            .Produces<GetPutAwayTaskResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Warehouse))
            .MapToApiVersion(1);

        // Search put-away tasks
        group.MapPost("/search", async (SearchPutAwayTasksCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPutAwayTasksEndpoint")
            .WithSummary("Search put-away tasks")
            .WithDescription("Searches put-away tasks with filtering, sorting, and pagination.")
            .Produces<PagedList<PutAwayTaskResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}

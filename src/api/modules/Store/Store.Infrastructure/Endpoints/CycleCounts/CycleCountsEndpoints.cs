using FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Cancel.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Complete.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Create.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Reconcile.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.RecordCount.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Start.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.CycleCounts;

/// <summary>
/// Endpoint configuration for Cycle Counts module.
/// </summary>
public class CycleCountsEndpoints() : CarterModule("store")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/cycle-counts").WithTags("cycle-counts");

        // Create cycle count
        group.MapPost("/", async (CreateCycleCountCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/cycle-counts/{result.Id}", result);
        })
        .WithName("CreateCycleCountEndpoint")
        .WithSummary("Create a new cycle count")
        .WithDescription("Schedules a new cycle count for a warehouse or location")
        .Produces<CreateCycleCountResponse>();

        // Get cycle count by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCycleCountRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetCycleCountEndpoint")
        .WithSummary("Get cycle count by ID")
        .WithDescription("Retrieves a cycle count by its unique identifier")
        .Produces<CycleCountResponse>();

        // Update cycle count
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCycleCountCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest("ID in URL does not match ID in request body");
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateCycleCountEndpoint")
        .WithSummary("Update a cycle count")
        .WithDescription("Updates cycle count details. Only cycle counts in 'Scheduled' status can be updated.")
        .Produces<UpdateCycleCountResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);

        // Search cycle counts
        group.MapPost("/search", async (SearchCycleCountsRequest request, ISender sender) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchCycleCountsEndpoint")
        .WithSummary("Search cycle counts")
        .WithDescription("Searches cycle counts with pagination and filters")
        .Produces<PagedList<CycleCountResponse>>();

        // Start cycle count
        group.MapPost("/{id:guid}/start", async (DefaultIdType id, StartCycleCountCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Cycle count ID mismatch");
            }
            
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("StartCycleCountEndpoint")
        .WithSummary("Start a cycle count")
        .WithDescription("Marks a scheduled cycle count as in-progress")
        .Produces<StartCycleCountResponse>();

        // Complete cycle count
        group.MapPost("/{id:guid}/complete", async (DefaultIdType id, CompleteCycleCountCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Cycle count ID mismatch");
            }
            
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("CompleteCycleCountEndpoint")
        .WithSummary("Complete a cycle count")
        .WithDescription("Marks an in-progress cycle count as completed and computes metrics")
        .Produces<CompleteCycleCountResponse>();

        // Cancel cycle count
        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelCycleCountCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Route ID does not match command ID");
            }

            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("CancelCycleCountEndpoint")
        .WithSummary("Cancel a cycle count")
        .WithDescription("Cancels a cycle count that is in 'Scheduled' or 'InProgress' status")
        .Produces<CancelCycleCountResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);

        // Reconcile cycle count
        group.MapPost("/{id:guid}/reconcile", async (DefaultIdType id, ReconcileCycleCountCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Cycle count ID mismatch");
            }
            
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("ReconcileCycleCountEndpoint")
        .WithSummary("Reconcile a cycle count")
        .WithDescription("Runs reconciliation for a completed cycle count and returns any discrepancies")
        .Produces<ReconcileCycleCountResponse>();

        // Add cycle count item
        group.MapPost("/{id:guid}/items", async (DefaultIdType id, AddCycleCountItemCommand command, ISender sender) =>
        {
            if (id != command.CycleCountId) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/cycle-counts/{result.CycleCountId}/items/{result.ItemId}", result);
        })
        .WithName("AddCycleCountItemEndpoint")
        .WithSummary("Add an item count to a cycle count")
        .WithDescription("Adds counted quantity for a grocery item to the cycle count")
        .Produces<AddCycleCountItemResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        // Record cycle count item
        group.MapPut("/{cycleCountId:guid}/items/{itemId:guid}/record", 
            async (DefaultIdType cycleCountId, DefaultIdType itemId, RecordCycleCountItemCommand command, ISender sender) =>
        {
            if (cycleCountId != command.CycleCountId || itemId != command.CycleCountItemId)
            {
                return Results.BadRequest("Route parameters do not match command properties");
            }

            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("RecordCycleCountItemEndpoint")
        .WithSummary("Record counted quantity for a cycle count item")
        .WithDescription("Records the physically counted quantity for a specific item during the counting phase. This is the core operation of cycle counting.")
        .Produces<RecordCycleCountItemResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);

        // Search cycle count items
        group.MapPost("/items/search", async (SearchCycleCountItemsRequest request, ISender sender) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchCycleCountItemsEndpoint")
        .WithSummary("Search cycle count items")
        .WithDescription("Search cycle count items with detailed item information for mobile counting")
        .Produces<PagedList<CycleCountItemDetailResponse>>();

        // Update cycle count item
        group.MapPut("/items/{id:guid}", async (DefaultIdType id, UpdateCycleCountItemCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateCycleCountItemEndpoint")
        .WithSummary("Update cycle count item")
        .WithDescription("Update the counted quantity and notes for a cycle count item")
        .Produces<DefaultIdType>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

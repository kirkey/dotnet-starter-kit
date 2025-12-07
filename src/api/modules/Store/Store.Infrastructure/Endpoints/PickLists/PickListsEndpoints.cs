using FSH.Starter.WebApi.Store.Application.PickLists.AddItem.v1;
using FSH.Starter.WebApi.Store.Application.PickLists.Assign.v1;
using FSH.Starter.WebApi.Store.Application.PickLists.Complete.v1;
using FSH.Starter.WebApi.Store.Application.PickLists.Create.v1;
using FSH.Starter.WebApi.Store.Application.PickLists.Delete.v1;
using FSH.Starter.WebApi.Store.Application.PickLists.Get.v1;
using FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;
using FSH.Starter.WebApi.Store.Application.PickLists.Start.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PickLists;

/// <summary>
/// Endpoint configuration for Pick Lists module.
/// </summary>
public class PickListsEndpoints() : CarterModule
{

    private const string AddPickListItemEndpoint = "AddPickListItemEndpoint";
    private const string AssignPickListEndpoint = "AssignPickListEndpoint";
    private const string CompletePickingEndpoint = "CompletePickingEndpoint";
    private const string CreatePickListEndpoint = "CreatePickListEndpoint";
    private const string DeletePickListEndpoint = "DeletePickListEndpoint";
    private const string GetPickListEndpoint = "GetPickListEndpoint";
    private const string SearchPickListsEndpoint = "SearchPickListsEndpoint";
    private const string StartPickingEndpoint = "StartPickingEndpoint";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/pick-lists").WithTags("pick-lists");

        // Create pick list
        group.MapPost("/", async (CreatePickListCommand request, ISender sender) =>
        {
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(CreatePickListEndpoint)
        .WithSummary("Create a new pick list")
        .WithDescription("Creates a new pick list for warehouse order fulfillment.")
        .Produces<CreatePickListResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        // Add item to pick list
        group.MapPost("/{id:guid}/items", async (DefaultIdType id, AddPickListItemCommand request, ISender sender) =>
        {
            if (id != request.PickListId)
            {
                return Results.BadRequest("Pick list ID in URL does not match request body");
            }

            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(AddPickListItemEndpoint)
        .WithSummary("Add item to pick list")
        .WithDescription("Adds an item to an existing pick list. The pick list must be in 'Created' status. Creates PickListItem as a separate aggregate.")
        .Produces<AddPickListItemResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Assign pick list to picker
        group.MapPost("/{id:guid}/assign", async (DefaultIdType id, AssignPickListCommand request, ISender sender) =>
        {
            if (id != request.PickListId)
            {
                return Results.BadRequest("Pick list ID mismatch");
            }

            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(AssignPickListEndpoint)
        .WithSummary("Assign pick list to picker")
        .WithDescription("Assigns a pick list to a warehouse picker.")
        .Produces<AssignPickListResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Start picking
        group.MapPost("/{id:guid}/start", async (DefaultIdType id, StartPickingCommand request, ISender sender) =>
        {
            if (id != request.PickListId)
            {
                return Results.BadRequest("Pick list ID mismatch");
            }
            
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(StartPickingEndpoint)
        .WithSummary("Start picking")
        .WithDescription("Marks a pick list as started and records the start time.")
        .Produces<StartPickingResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Complete picking
        group.MapPost("/{id:guid}/complete", async (DefaultIdType id, CompletePickingCommand request, ISender sender) =>
        {
            if (id != request.PickListId)
            {
                return Results.BadRequest("Pick list ID mismatch");
            }
            
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(CompletePickingEndpoint)
        .WithSummary("Complete picking")
        .WithDescription("Marks a pick list as completed and records the completion time.")
        .Produces<CompletePickingResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Delete pick list
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var request = new DeletePickListCommand { PickListId = id };
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(DeletePickListEndpoint)
        .WithSummary("Delete a pick list")
        .WithDescription("Deletes an existing pick list.")
        .Produces<DeletePickListResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        // Get pick list by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var command = new GetPickListCommand(id);
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(GetPickListEndpoint)
        .WithSummary("Get pick list by ID")
        .WithDescription("Retrieves a specific pick list with all items.")
        .Produces<GetPickListResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Search pick lists
        group.MapPost("/search", async (SearchPickListsCommand command, ISender sender) =>
        {
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(SearchPickListsEndpoint)
        .WithSummary("Search pick lists")
        .WithDescription("Searches pick lists with pagination and filtering options.")
        .Produces<PagedList<PickListResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}

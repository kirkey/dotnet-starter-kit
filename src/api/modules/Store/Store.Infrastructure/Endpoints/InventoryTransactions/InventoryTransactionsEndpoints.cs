using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Approve.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Create.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Delete.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Reject.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Search.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransactions.UpdateNotes.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransactions;

/// <summary>
/// Endpoint configuration for Inventory Transactions module using Carter.
/// </summary>
public class InventoryTransactionsEndpoints() : CarterModule("store")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/inventory-transactions").WithTags("inventory-transactions");

        // Create inventory transaction
        group.MapPost("/", async (CreateInventoryTransactionCommand request, ISender sender) =>
        {
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateInventoryTransaction")
        .WithSummary("Create a new inventory transaction")
        .WithDescription("Creates a new inventory transaction for stock movement tracking and audit trail.")
        .Produces<CreateInventoryTransactionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        // Approve inventory transaction
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveInventoryTransactionCommand request, ISender sender) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID mismatch between route and body.");
            }

            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("ApproveInventoryTransaction")
        .WithSummary("Approve an inventory transaction")
        .WithDescription("Approves a pending inventory transaction for authorization and compliance.")
        .Produces<ApproveInventoryTransactionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Reject inventory transaction
        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectInventoryTransactionCommand request, ISender sender) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID mismatch between route and body.");
            }

            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("RejectInventoryTransaction")
        .WithSummary("Reject an inventory transaction")
        .WithDescription("Rejects a previously approved inventory transaction, optionally recording the reason for rejection.")
        .Produces<RejectInventoryTransactionResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Update inventory transaction notes
        group.MapPatch("/{id:guid}/notes", async (DefaultIdType id, UpdateInventoryTransactionNotesCommand request, ISender sender) =>
        {
            request.Id = id;
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateInventoryTransactionNotes")
        .WithSummary("Update inventory transaction notes")
        .WithDescription("Updates the notes field on an existing inventory transaction for additional documentation.")
        .Produces<UpdateInventoryTransactionNotesResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Delete inventory transaction
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new DeleteInventoryTransactionCommand { Id = id }).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("DeleteInventoryTransaction")
        .WithSummary("Delete an inventory transaction")
        .WithDescription("Deletes an existing inventory transaction from the system.")
        .Produces<DeleteInventoryTransactionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        // Get inventory transaction by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetInventoryTransactionCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetInventoryTransaction")
        .WithSummary("Get an inventory transaction by ID")
        .WithDescription("Retrieves a specific inventory transaction by its unique identifier.")
        .Produces<InventoryTransactionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Search inventory transactions
        group.MapPost("/search", async (SearchInventoryTransactionsCommand command, ISender sender) =>
        {
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchInventoryTransactions")
        .WithSummary("Search inventory transactions")
        .WithDescription("Searches for inventory transactions with pagination and filtering by transaction number, item, warehouse, type, date range, approval status, and cost range.")
        .Produces<PagedList<InventoryTransactionDto>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}

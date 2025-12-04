using Accounting.Application.DeferredRevenues.Create;
using Accounting.Application.DeferredRevenues.Delete;
using Accounting.Application.DeferredRevenues.Get;
using Accounting.Application.DeferredRevenues.Recognize;
using Accounting.Application.DeferredRevenues.Responses;
using Accounting.Application.DeferredRevenues.Search;
using Accounting.Application.DeferredRevenues.Update;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues;

/// <summary>
/// Endpoint configuration for Deferred Revenue module.
/// </summary>
public class DeferredRevenuesEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Deferred Revenue endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/deferred-revenues").WithTags("deferred-revenues");

        // Create endpoint
        group.MapPost("/", async (CreateDeferredRevenueCommand command, ISender mediator) =>
            {
                var id = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/api/v1/deferred-revenues/{id}", new { Id = id });
            })
            .WithName("CreateDeferredRevenue")
            .WithSummary("Create a new deferred revenue entry")
            .WithDescription("Creates a new deferred revenue entry for revenue recognition tracking")
            .Produces<object>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDeferredRevenueRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetDeferredRevenue")
            .WithSummary("Get deferred revenue by ID")
            .WithDescription("Retrieves a deferred revenue entry by its unique identifier")
            .Produces<DeferredRevenueResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateDeferredRevenueCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = result });
            })
            .WithName("UpdateDeferredRevenue")
            .WithSummary("Update deferred revenue")
            .WithDescription("Updates an existing deferred revenue entry (cannot update recognized revenue)")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteDeferredRevenueCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = result });
            })
            .WithName("DeleteDeferredRevenue")
            .WithSummary("Delete deferred revenue")
            .WithDescription("Deletes a deferred revenue entry (cannot delete recognized revenue)")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async (SearchDeferredRevenuesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchDeferredRevenues")
            .WithSummary("Search deferred revenues")
            .WithDescription("Searches deferred revenue entries with filtering and pagination")
            .Produces<PagedList<DeferredRevenueResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Recognize endpoint
        group.MapPost("/{id:guid}/recognize", async (DefaultIdType id, RecognizeDeferredRevenueCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = result, Message = "Deferred revenue recognized successfully" });
            })
            .WithName("RecognizeDeferredRevenue")
            .WithSummary("Recognize deferred revenue")
            .WithDescription("Marks deferred revenue as recognized, preventing further modifications")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting));
    }
}


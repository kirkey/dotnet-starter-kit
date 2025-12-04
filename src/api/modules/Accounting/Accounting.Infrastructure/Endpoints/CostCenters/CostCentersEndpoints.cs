using Accounting.Application.CostCenters.Activate.v1;
using Accounting.Application.CostCenters.Create.v1;
using Accounting.Application.CostCenters.Deactivate.v1;
using Accounting.Application.CostCenters.Delete.v1;
using Accounting.Application.CostCenters.Get;
using Accounting.Application.CostCenters.RecordActual.v1;
using Accounting.Application.CostCenters.Responses;
using Accounting.Application.CostCenters.Search.v1;
using Accounting.Application.CostCenters.Update.v1;
using Accounting.Application.CostCenters.UpdateBudget.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters;

/// <summary>
/// Endpoint configuration for Cost Centers module.
/// </summary>
public class CostCentersEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Cost Centers endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/cost-centers").WithTags("cost-centers");

        // Create endpoint
        group.MapPost("/", async (CostCenterCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/cost-centers/{response.Id}", response);
            })
            .WithName("CreateCostCenter")
            .WithSummary("Create cost center")
            .Produces<CostCenterCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCostCenterRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetCostCenter")
            .WithSummary("Get cost center by ID")
            .WithDescription("Retrieves a cost center by its unique identifier")
            .Produces<CostCenterResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCostCenterCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId });
            })
            .WithName("UpdateCostCenter")
            .WithSummary("Update cost center")
            .WithDescription("Updates a cost center details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Delete endpoint
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteCostCenterCommand(id);
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("DeleteCostCenter")
            .WithSummary("Delete cost center")
            .WithDescription("Deletes an inactive cost center with no transactions")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (SearchCostCentersRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchCostCenters")
            .WithSummary("Search cost centers")
            .Produces<PagedList<CostCenterResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update Budget endpoint
        group.MapPut("/{id:guid}/budget", async (DefaultIdType id, UpdateCostCenterBudgetCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");
                    
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId });
            })
            .WithName("UpdateCostCenterBudget")
            .WithSummary("Update cost center budget")
            .WithDescription("Updates the budget allocation for a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Record Actual endpoint
        group.MapPost("/{id:guid}/actual", async (DefaultIdType id, RecordCostCenterActualCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId });
            })
            .WithName("RecordCostCenterActual")
            .WithSummary("Record cost center actual expenses")
            .WithDescription("Records actual expenses/costs for a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);

        // Activate endpoint
        group.MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender mediator) =>
            {
                var costCenterId = await mediator.Send(new ActivateCostCenterCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId, Message = "Cost center activated successfully" });
            })
            .WithName("ActivateCostCenter")
            .WithSummary("Activate cost center")
            .WithDescription("Activates a cost center for use")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Deactivate endpoint
        group.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender mediator) =>
            {
                var costCenterId = await mediator.Send(new DeactivateCostCenterCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId, Message = "Cost center deactivated successfully" });
            })
            .WithName("DeactivateCostCenter")
            .WithSummary("Deactivate cost center")
            .WithDescription("Deactivates a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

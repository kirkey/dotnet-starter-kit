using Accounting.Application.Budgets.Approve;
using Accounting.Application.Budgets.Close;
using Accounting.Application.Budgets.Create;
using Accounting.Application.Budgets.Delete;
using Accounting.Application.Budgets.Get;
using Accounting.Application.Budgets.Responses;
using Accounting.Application.Budgets.Search;
using Accounting.Application.Budgets.Update;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Budgets;

/// <summary>
/// Endpoint configuration for Budgets module.
/// </summary>
public class BudgetsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Budgets endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/budgets").WithTags("budgets");

        // Create endpoint
        group.MapPost("/", async (CreateBudgetCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateBudget")
            .WithSummary("create a budget")
            .WithDescription("create a budget")
            .Produces<CreateBudgetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBudgetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBudget")
            .WithSummary("get a budget by id")
            .WithDescription("get a budget by id")
            .Produces<BudgetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateBudgetCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateBudget")
            .WithSummary("update a budget")
            .WithDescription("update a budget")
            .Produces<UpdateBudgetResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBudgetCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteBudget")
            .WithSummary("delete budget by id")
            .WithDescription("delete budget by id")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async (ISender mediator, [FromBody] SearchBudgetsRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBudgets")
            .WithSummary("Gets a list of budgets")
            .WithDescription("Gets a list of budgets with pagination and filtering support")
            .Produces<PagedList<BudgetResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Approve endpoint
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveBudgetCommand command, ISender mediator) =>
            {
                if (id != command.BudgetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("ApproveBudget")
            .WithSummary("Approve budget")
            .WithDescription("Approves a budget for activation")
            .Produces<DefaultIdType>();

        // Close endpoint
        group.MapPost("/{id:guid}/close", async (DefaultIdType id, CloseBudgetCommand command, ISender mediator) =>
            {
                if (id != command.BudgetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("CloseBudget")
            .WithSummary("Close budget")
            .WithDescription("Closes a budget")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting));
    }
}

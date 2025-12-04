using Accounting.Application.Budgets.Details.Create;
using Accounting.Application.Budgets.Details.Delete;
using Accounting.Application.Budgets.Details.Get;
using Accounting.Application.Budgets.Details.Responses;
using Accounting.Application.Budgets.Details.Search;
using Accounting.Application.Budgets.Details.Update;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BudgetDetails;

/// <summary>
/// Endpoint configuration for Budget Details module.
/// </summary>
public class BudgetDetailsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Budget Details endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/budget-details").WithTags("budget-details");

        // Create endpoint
        group.MapPost("/", async (CreateBudgetDetailCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request);
                return Results.Ok(id);
            })
            .WithName("CreateBudgetDetail")
            .WithSummary("create budget detail")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBudgetDetailQuery(id));
                return Results.Ok(response);
            })
            .WithName("GetBudgetDetail")
            .WithSummary("get budget detail by id")
            .Produces<BudgetDetailResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateBudgetDetailCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var updatedId = await mediator.Send(command);
                return Results.Ok(updatedId);
            })
            .WithName("UpdateBudgetDetail")
            .WithSummary("update budget detail")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBudgetDetailCommand(id));
                return Results.NoContent();
            })
            .WithName("DeleteBudgetDetail")
            .WithSummary("delete budget detail")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search by budget ID endpoint
        group.MapGet("/by-budget/{budgetId:guid}", async (DefaultIdType budgetId, ISender mediator) =>
            {
                var list = await mediator.Send(new SearchBudgetDetailsByBudgetIdQuery(budgetId));
                return Results.Ok(list);
            })
            .WithName("SearchBudgetDetailsByBudgetId")
            .WithSummary("list budget details by budget id")
            .Produces<List<BudgetDetailResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));
    }
}

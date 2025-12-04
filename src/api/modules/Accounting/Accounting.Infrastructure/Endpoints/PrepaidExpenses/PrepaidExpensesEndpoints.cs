using Accounting.Application.PrepaidExpenses.Cancel.v1;
using Accounting.Application.PrepaidExpenses.Close.v1;
using Accounting.Application.PrepaidExpenses.Create.v1;
using Accounting.Application.PrepaidExpenses.Get;
using Accounting.Application.PrepaidExpenses.RecordAmortization.v1;
using Accounting.Application.PrepaidExpenses.Responses;
using Accounting.Application.PrepaidExpenses.Search.v1;
using Accounting.Application.PrepaidExpenses.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses;

/// <summary>
/// Endpoint configuration for Prepaid Expenses module.
/// Provides comprehensive REST API endpoints for managing prepaid expenses.
/// </summary>
public class PrepaidExpensesEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Prepaid Expenses endpoints to the route builder.
    /// Includes Create, Read, Update, Search, and workflow operations.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/prepaid-expenses").WithTags("prepaid-expenses");

        // Create endpoint
        group.MapPost("/", async (PrepaidExpenseCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/prepaid-expenses/{response.Id}", response);
            })
            .WithName("CreatePrepaidExpense")
            .WithSummary("Create prepaid expense")
            .Produces<PrepaidExpenseCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPrepaidExpenseRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPrepaidExpense")
            .WithSummary("Get prepaid expense by ID")
            .WithDescription("Retrieves a prepaid expense by its unique identifier")
            .Produces<PrepaidExpenseResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePrepaidExpenseCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId });
            })
            .WithName("UpdatePrepaidExpense")
            .WithSummary("Update prepaid expense")
            .WithDescription("Updates a prepaid expense details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (SearchPrepaidExpensesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPrepaidExpenses")
            .WithSummary("Search prepaid expenses")
            .WithDescription("Search prepaid expenses with filtering and pagination")
            .Produces<PagedList<PrepaidExpenseResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Record amortization endpoint
        group.MapPost("/{id:guid}/amortization", async (DefaultIdType id, RecordAmortizationCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId, Message = "Amortization recorded successfully" });
            })
            .WithName("RecordPrepaidExpenseAmortization")
            .WithSummary("Record amortization")
            .WithDescription("Records an amortization posting for the period")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);

        // Close endpoint
        group.MapPost("/{id:guid}/close", async (DefaultIdType id, ClosePrepaidExpenseCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId, Message = "Prepaid expense closed successfully" });
            })
            .WithName("ClosePrepaidExpense")
            .WithSummary("Close prepaid expense")
            .WithDescription("Closes a fully amortized prepaid expense")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Cancel endpoint
        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelPrepaidExpenseCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId, Message = "Prepaid expense cancelled successfully" });
            })
            .WithName("CancelPrepaidExpense")
            .WithSummary("Cancel prepaid expense")
            .WithDescription("Cancels a prepaid expense without amortization history")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


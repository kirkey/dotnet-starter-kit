using Accounting.Application.Checks.Clear.v1;
using Accounting.Application.Checks.Create.v1;
using Accounting.Application.Checks.Get.v1;
using Accounting.Application.Checks.Issue.v1;
using Accounting.Application.Checks.Print.v1;
using Accounting.Application.Checks.Search.v1;
using Accounting.Application.Checks.StopPayment.v1;
using Accounting.Application.Checks.Update.v1;
using Accounting.Application.Checks.Void.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Checks;

/// <summary>
/// Endpoint configuration for Checks module.
/// Provides comprehensive REST API endpoints for managing checks including creation, issuance, payment stops, and reconciliation.
/// </summary>
public class ChecksEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Checks endpoints to the route builder.
    /// Includes Create, Read, Search, and state transition operations (Issue, Void, Clear, Stop Payment, Print).
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/checks").WithTags("checks");

        // Create endpoint
        group.MapPost("/", async (CheckCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/checks/{response.Id}", response);
            })
            .WithName("CreateCheck")
            .WithSummary("Register a new check")
            .WithDescription("Register a new check in the system for later use in payments. Creates a check in 'Available' status.")
            .Produces<CheckCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new CheckGetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetCheck")
            .WithSummary("Get check by ID")
            .WithDescription("Retrieve detailed information about a specific check including current status, payment details, and complete audit trail.")
            .Produces<CheckGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, CheckUpdateCommand command, ISender mediator) =>
            {
                if (id != command.CheckId)
                {
                    return Results.BadRequest("The ID in the URL does not match the ID in the request body.");
                }

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateCheck")
            .WithSummary("Update an existing check")
            .WithDescription("Updates an existing check in the accounting system. BankAccountName and BankName are automatically populated from their respective entities based on the provided BankAccountCode and BankId.")
            .Produces<CheckUpdateResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (CheckSearchQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchChecks")
            .WithSummary("Search checks")
            .WithDescription("Search and filter checks with pagination, status filtering, date ranges, and amount ranges. Supports advanced filtering by check number, bank account, payee name, and print/stop payment status.")
            .Produces<PagedList<CheckSearchResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Issue endpoint
        group.MapPost("/issue", async (CheckIssueCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("IssueCheck")
            .WithSummary("Issue a check for payment")
            .WithDescription("Issue a check to a payee/vendor for payment of expenses, invoices, or other obligations. Transitions check to Issued status.")
            .Produces<CheckIssueResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Void endpoint
        group.MapPost("/void", async (CheckVoidCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("VoidCheck")
            .WithSummary("Void a check")
            .WithDescription("Void a check that was issued in error or needs to be cancelled. Records the void reason in audit trail.")
            .Produces<CheckVoidResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Clear endpoint
        group.MapPost("/clear", async (CheckClearCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ClearCheck")
            .WithSummary("Mark check as cleared")
            .WithDescription("Mark a check as cleared during bank reconciliation when it appears on the bank statement.")
            .Produces<CheckClearResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Stop Payment endpoint
        group.MapPost("/stop-payment", async (CheckStopPaymentCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("StopPaymentCheck")
            .WithSummary("Request stop payment on check")
            .WithDescription("Request stop payment on a check that is lost, stolen, or needs to be cancelled before presentation to the bank.")
            .Produces<CheckStopPaymentResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Print endpoint
        group.MapPost("/print", async (CheckPrintCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("PrintCheck")
            .WithSummary("Mark check as printed")
            .WithDescription("Mark a check as printed and record who performed the printing. Maintains audit trail for compliance.")
            .Produces<CheckPrintResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

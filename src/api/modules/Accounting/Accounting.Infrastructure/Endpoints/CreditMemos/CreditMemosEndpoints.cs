using Accounting.Application.CreditMemos.Apply;
using Accounting.Application.CreditMemos.Approve;
using Accounting.Application.CreditMemos.Create;
using Accounting.Application.CreditMemos.Delete;
using Accounting.Application.CreditMemos.Get;
using Accounting.Application.CreditMemos.Refund;
using Accounting.Application.CreditMemos.Responses;
using Accounting.Application.CreditMemos.Search;
using Accounting.Application.CreditMemos.Update;
using Accounting.Application.CreditMemos.Void;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CreditMemos;

/// <summary>
/// Endpoint configuration for Credit Memos module.
/// </summary>
public class CreditMemosEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Credit Memos endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/credit-memos").WithTags("credit-memos");

        // Create endpoint
        group.MapPost("/", async (CreateCreditMemoCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateCreditMemo")
            .WithSummary("Create a credit memo")
            .WithDescription("Create a new credit memo for receivable/payable adjustments")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var query = new GetCreditMemoQuery(id);
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetCreditMemo")
            .WithSummary("Get credit memo by ID")
            .WithDescription("Retrieve a specific credit memo by its identifier")
            .Produces<CreditMemoResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("UpdateCreditMemo")
            .WithSummary("Update a credit memo")
            .WithDescription("Update an existing credit memo (draft only)")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteCreditMemoCommand(id);
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteCreditMemo")
            .WithSummary("Delete a credit memo")
            .WithDescription("Delete a credit memo (draft status only)")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (ISender mediator, [FromBody] SearchCreditMemosQuery query) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchCreditMemos")
            .WithSummary("Search credit memos")
            .WithDescription("Search and filter credit memos with pagination")
            .Produces<PagedList<CreditMemoResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Approve endpoint
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("ApproveCreditMemo")
            .WithSummary("Approve a credit memo")
            .WithDescription("Approve a draft credit memo for application or refund")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting))
            .MapToApiVersion(1);

        // Apply endpoint
        group.MapPost("/{id:guid}/apply", async (DefaultIdType id, ApplyCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("ApplyCreditMemo")
            .WithSummary("Apply a credit memo")
            .WithDescription("Apply an approved credit memo to an invoice or bill")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Refund endpoint
        group.MapPost("/{id:guid}/refund", async (DefaultIdType id, RefundCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("RefundCreditMemo")
            .WithSummary("Issue refund for credit memo")
            .WithDescription("Issue a direct refund for an approved credit memo")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Void endpoint
        group.MapPost("/{id:guid}/void", async (DefaultIdType id, VoidCreditMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("VoidCreditMemo")
            .WithSummary("Void a credit memo")
            .WithDescription("Void a credit memo and reverse any applications or refunds")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

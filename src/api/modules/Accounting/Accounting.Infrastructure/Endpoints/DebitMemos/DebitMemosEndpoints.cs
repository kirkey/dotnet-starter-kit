using Accounting.Application.DebitMemos.Apply;
using Accounting.Application.DebitMemos.Approve;
using Accounting.Application.DebitMemos.Create;
using Accounting.Application.DebitMemos.Delete;
using Accounting.Application.DebitMemos.Get;
using Accounting.Application.DebitMemos.Responses;
using Accounting.Application.DebitMemos.Search;
using Accounting.Application.DebitMemos.Update;
using Accounting.Application.DebitMemos.Void;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DebitMemos;

/// <summary>
/// Endpoint configuration for Debit Memos module.
/// </summary>
public class DebitMemosEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Debit Memos endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/debit-memos").WithTags("debit-memos");

        // Create endpoint
        group.MapPost("/", async (CreateDebitMemoCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateDebitMemo")
            .WithSummary("Create a debit memo")
            .WithDescription("Create a new debit memo for receivable/payable adjustments")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var query = new GetDebitMemoQuery(id);
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetDebitMemo")
            .WithSummary("Get debit memo by ID")
            .WithDescription("Retrieve a specific debit memo by its identifier")
            .Produces<DebitMemoResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("UpdateDebitMemo")
            .WithSummary("Update a debit memo")
            .WithDescription("Update an existing debit memo (draft only)")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteDebitMemoCommand(id);
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteDebitMemo")
            .WithSummary("Delete a debit memo")
            .WithDescription("Delete a debit memo (draft status only)")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async (ISender mediator, [FromBody] SearchDebitMemosQuery query) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchDebitMemos")
            .WithSummary("Search debit memos")
            .WithDescription("Search and filter debit memos with pagination")
            .Produces<PagedList<DebitMemoResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Approve endpoint
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("ApproveDebitMemo")
            .WithSummary("Approve a debit memo")
            .WithDescription("Approve a draft debit memo for application")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting));

        // Apply endpoint
        group.MapPost("/{id:guid}/apply", async (DefaultIdType id, ApplyDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("ApplyDebitMemo")
            .WithSummary("Apply a debit memo")
            .WithDescription("Apply an approved debit memo to an invoice or bill")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Void endpoint
        group.MapPost("/{id:guid}/void", async (DefaultIdType id, VoidDebitMemoCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("VoidDebitMemo")
            .WithSummary("Void a debit memo")
            .WithDescription("Void a debit memo and reverse any applications")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));
    }
}

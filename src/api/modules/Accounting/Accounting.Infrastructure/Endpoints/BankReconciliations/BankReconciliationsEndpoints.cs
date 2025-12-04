using Accounting.Application.BankReconciliations.Approve.v1;
using Accounting.Application.BankReconciliations.Complete.v1;
using Accounting.Application.BankReconciliations.Create.v1;
using Accounting.Application.BankReconciliations.Delete.v1;
using Accounting.Application.BankReconciliations.Get.v1;
using Accounting.Application.BankReconciliations.Reject.v1;
using Accounting.Application.BankReconciliations.Responses;
using Accounting.Application.BankReconciliations.Search.v1;
using Accounting.Application.BankReconciliations.Start.v1;
using Accounting.Application.BankReconciliations.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations;

/// <summary>
/// Endpoint configuration for Bank Reconciliations module.
/// </summary>
public class BankReconciliationsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Bank Reconciliations endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/bank-reconciliations").WithTags("bank-reconciliations");

        group.MapPost("/", async (CreateBankReconciliationCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/bank-reconciliations/{response}", response);
            })
            .WithName("CreateBankReconciliation")
            .WithSummary("Create a new bank reconciliation")
            .WithDescription("Create a new bank reconciliation with opening balances from bank statement and general ledger. " +
                "Reconciliation starts in Pending status and moves through InProgress, Completed, and Approved statuses.")
            .Produces<DefaultIdType>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBankReconciliationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBankReconciliation")
            .WithSummary("Get bank reconciliation details")
            .WithDescription("Retrieve a specific bank reconciliation with all its details including status, balances, " +
                "adjustments, and audit information.")
            .Produces<BankReconciliationResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch between URL parameter and request body.");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("UpdateBankReconciliation")
            .WithSummary("Update reconciliation items")
            .WithDescription("Update outstanding checks, deposits in transit, and error adjustments for an in-progress reconciliation. " +
                "Calculates adjusted book balance which must eventually match the statement balance.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBankReconciliationCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteBankReconciliation")
            .WithSummary("Delete a bank reconciliation")
            .WithDescription("Delete a bank reconciliation. Only reconciliations that are not yet reconciled and approved can be deleted. " +
                "This permanently removes the reconciliation record.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        group.MapPost("/search", async (SearchBankReconciliationsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBankReconciliations")
            .WithSummary("Search bank reconciliations")
            .WithDescription("Search and filter bank reconciliations by bank account, date range, status, and reconciliation state. " +
                "Supports pagination. Results are ordered by reconciliation date (most recent first).")
            .Produces<PagedList<BankReconciliationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        group.MapPost("/{id}/start", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new StartBankReconciliationCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("StartBankReconciliation")
            .WithSummary("Start a bank reconciliation")
            .WithDescription("Transition a bank reconciliation from Pending to InProgress status. " +
                "Once started, the user can begin entering reconciliation items such as outstanding checks " +
                "and deposits in transit.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.Accounting));

        group.MapPost("/{id}/complete", async (DefaultIdType id, CompleteBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch between URL parameter and request body.");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("CompleteBankReconciliation")
            .WithSummary("Complete a bank reconciliation")
            .WithDescription("Mark a bank reconciliation as completed. Verifies that the adjusted book balance equals " +
                "the statement balance (within tolerance). Records who completed the reconciliation for audit purposes.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting));

        group.MapPost("/{id}/approve", async (DefaultIdType id, ApproveBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch between URL parameter and request body.");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("ApproveBankReconciliation")
            .WithSummary("Approve a bank reconciliation")
            .WithDescription("Approve a completed bank reconciliation, marking it as final and verified. " +
                "Only reconciliations with Completed status can be approved. Sets IsReconciled to true.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting));

        group.MapPost("/{id}/reject", async (DefaultIdType id, RejectBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch between URL parameter and request body.");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("RejectBankReconciliation")
            .WithSummary("Reject a bank reconciliation")
            .WithDescription("Reject a completed bank reconciliation and return it to Pending status for rework. " +
                "The rejection reason is recorded in the reconciliation notes for reference.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting));
    }
}

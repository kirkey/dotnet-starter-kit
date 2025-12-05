using Accounting.Application.AccountReconciliations.Approve.v1;
using Accounting.Application.AccountReconciliations.Commands.ReconcileAccount.v1;
using Accounting.Application.AccountReconciliations.Create.v1;
using Accounting.Application.AccountReconciliations.Delete.v1;
using Accounting.Application.AccountReconciliations.Get.v1;
using Accounting.Application.AccountReconciliations.Responses;
using Accounting.Application.AccountReconciliations.Search.v1;
using Accounting.Application.AccountReconciliations.Update.v1;
using Carter;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations;

/// <summary>
/// Endpoint configuration for Account Reconciliations module.
/// </summary>
public class AccountReconciliationEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Account Reconciliation endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/account-reconciliations").WithTags("account-reconciliations");

        group.MapPost("/", async (CreateAccountReconciliationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/account-reconciliations/{response}", null);
            })
            .WithName("CreateAccountReconciliation")
            .WithSummary("Create Account Reconciliation")
            .WithDescription("Create a new account reconciliation comparing GL balance with subsidiary ledger")
            .Produces<DefaultIdType>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAccountReconciliationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetAccountReconciliation")
            .WithSummary("Get Account Reconciliation")
            .WithDescription("Get account reconciliation details by ID")
            .Produces<AccountReconciliationResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPost("/search", async (ISender mediator, [FromBody] SearchAccountReconciliationsRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchAccountReconciliations")
            .WithSummary("Search Account Reconciliations")
            .WithDescription("Search and filter account reconciliations with pagination and filtering support")
            .Produces<PagedList<AccountReconciliationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, ISender mediator, UpdateAccountReconciliationCommand command) =>
            {
                await mediator.Send(command with { Id = id }).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("UpdateAccountReconciliation")
            .WithSummary("Update Account Reconciliation")
            .WithDescription("Update reconciliation balances and metadata")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPost("/{id}/approve", async (DefaultIdType id, ISender mediator, ApproveAccountReconciliationCommand command) =>
            {
                await mediator.Send(command with { Id = id }).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("ApproveAccountReconciliation")
            .WithSummary("Approve Account Reconciliation")
            .WithDescription("Approve a reconciled account reconciliation")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteAccountReconciliationCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteAccountReconciliation")
            .WithSummary("Delete Account Reconciliation")
            .WithDescription("Delete an account reconciliation (only if not approved)")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPost("/reconcile", async (ReconcileGeneralLedgerAccountCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName("ReconcileGeneralLedgerAccount")
            .WithSummary("Reconcile a general ledger account")
            .WithDescription("Run account reconciliation for a chart of account and its reconciliation lines")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


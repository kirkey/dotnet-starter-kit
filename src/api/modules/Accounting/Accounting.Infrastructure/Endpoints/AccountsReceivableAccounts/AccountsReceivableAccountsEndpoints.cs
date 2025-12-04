using Accounting.Application.AccountsReceivableAccounts.Create.v1;
using Accounting.Application.AccountsReceivableAccounts.Get;
using Accounting.Application.AccountsReceivableAccounts.Reconcile.v1;
using Accounting.Application.AccountsReceivableAccounts.RecordCollection.v1;
using Accounting.Application.AccountsReceivableAccounts.RecordWriteOff.v1;
using Accounting.Application.AccountsReceivableAccounts.Responses;
using Accounting.Application.AccountsReceivableAccounts.Search.v1;
using Accounting.Application.AccountsReceivableAccounts.UpdateAllowance.v1;
using Accounting.Application.AccountsReceivableAccounts.UpdateBalance.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts;

/// <summary>
/// Endpoint configuration for Accounts Receivable Accounts module.
/// </summary>
public class AccountsReceivableAccountsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Accounts Receivable Accounts endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accounts-receivable-accounts").WithTags("accounts-receivable-accounts");

        // CRUD operations
        group.MapPost("/", async (AccountsReceivableAccountCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/accounts-receivable/{response.Id}", response);
            })
            .WithName("CreateArAccount")
            .WithSummary("Create AR account")
            .Produces<AccountsReceivableAccountCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetArAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetArAccount")
            .WithSummary("Get AR account by ID")
            .Produces<ArAccountResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        group.MapPost("/search", async (SearchArAccountsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchArAccounts")
            .WithSummary("Search AR accounts")
            .Produces<PagedList<ArAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Workflow operations
        group.MapPut("/{id:guid}/balance", async (DefaultIdType id, UpdateArBalanceCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId });
            })
            .WithName("UpdateArAccountBalance")
            .WithSummary("Update AR aging balance")
            .WithDescription("Updates the aging buckets and calculates total balance")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        group.MapPut("/{id:guid}/allowance", async (DefaultIdType id, UpdateARAllowanceCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId });
            })
            .WithName("UpdateArAccountAllowance")
            .WithSummary("Update allowance for doubtful accounts")
            .WithDescription("Updates the allowance for uncollectible receivables")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        group.MapPost("/{id:guid}/write-offs", async (DefaultIdType id, RecordARWriteOffCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Write-off recorded successfully" });
            })
            .WithName("RecordArWriteOff")
            .WithSummary("Record AR bad debt write-off")
            .WithDescription("Records a bad debt write-off for an AR account and updates bad debt statistics")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting));

        group.MapPost("/{id:guid}/collections", async (DefaultIdType id, RecordARCollectionCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Collection recorded successfully" });
            })
            .WithName("RecordArCollection")
            .WithSummary("Record AR collection")
            .WithDescription("Records a collection (payment received) for an AR account and updates YTD statistics")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Receive, FshResources.Accounting));

        group.MapPost("/{id:guid}/reconcile", async (DefaultIdType id, ReconcileArAccountCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Reconciliation completed successfully" });
            })
            .WithName("ReconcileArAccount")
            .WithSummary("Reconcile with subsidiary ledger")
            .WithDescription("Reconciles AR control account with subsidiary customer ledgers")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));
    }
}

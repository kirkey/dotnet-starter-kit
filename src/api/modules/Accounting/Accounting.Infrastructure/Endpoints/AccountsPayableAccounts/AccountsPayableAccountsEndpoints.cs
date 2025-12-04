using Accounting.Application.AccountsPayableAccounts.Create.v1;
using Accounting.Application.AccountsPayableAccounts.Delete.v1;
using Accounting.Application.AccountsPayableAccounts.Get;
using Accounting.Application.AccountsPayableAccounts.Reconcile.v1;
using Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;
using Accounting.Application.AccountsPayableAccounts.RecordPayment.v1;
using Accounting.Application.AccountsPayableAccounts.Responses;
using Accounting.Application.AccountsPayableAccounts.Search.v1;
using Accounting.Application.AccountsPayableAccounts.Update.v1;
using Accounting.Application.AccountsPayableAccounts.UpdateBalance.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts;

/// <summary>
/// Endpoint configuration for Accounts Payable Accounts module.
/// </summary>
public class AccountsPayableAccountsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Accounts Payable Accounts endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accounts-payable-accounts").WithTags("accounts-payable-accounts");

        // CRUD operations
        group.MapPost("/", async (AccountsPayableAccountCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/accounts-payable/{response.Id}", response);
            })
            .WithName("CreateApAccount")
            .WithSummary("Create AP account")
            .Produces<AccountsPayableAccountCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAPAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetApAccount")
            .WithSummary("Get AP account by ID")
            .Produces<ApAccountResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        group.MapPut("/{id}", async (DefaultIdType id, AccountsPayableAccountUpdateCommand request, ISender mediator) =>
            {
                request.GetType().GetProperty("Id")?.SetValue(request, id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateApAccount")
            .WithSummary("Update AP account")
            .WithDescription("Updates an existing accounts payable account")
            .Produces<AccountsPayableAccountUpdateResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new AccountsPayableAccountDeleteCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteApAccount")
            .WithSummary("Delete AP account")
            .WithDescription("Deletes an accounts payable account (only if balance is zero)")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        group.MapPost("/search", async (SearchApAccountsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchApAccounts")
            .WithSummary("Search AP accounts")
            .Produces<PagedList<ApAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Workflow operations
        group.MapPut("/{id:guid}/balance", async (DefaultIdType id, UpdateAPBalanceCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId });
            })
            .WithName("UpdateApAccountBalance")
            .WithSummary("Update AP aging balance")
            .WithDescription("Updates the aging buckets and calculates total balance")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        group.MapPost("/{id:guid}/payments", async (DefaultIdType id, RecordAPPaymentCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Payment recorded successfully" });
            })
            .WithName("RecordApPayment")
            .WithSummary("Record vendor payment")
            .WithDescription("Records a payment to vendors and tracks early payment discounts")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting));

        group.MapPost("/{id:guid}/discounts-lost", async (DefaultIdType id, RecordAPDiscountLostCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Discount lost recorded successfully" });
            })
            .WithName("RecordApDiscountLost")
            .WithSummary("Record missed early payment discount")
            .WithDescription("Records a missed early payment discount opportunity for an AP account")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting));

        group.MapPost("/{id:guid}/reconcile", async (DefaultIdType id, ReconcileAPAccountCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Reconciliation completed successfully" });
            })
            .WithName("ReconcileApAccount")
            .WithSummary("Reconcile with subsidiary ledger")
            .WithDescription("Reconciles AP control account with subsidiary vendor ledgers")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));
    }
}

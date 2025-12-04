using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Transfer.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Withdraw.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Savings Accounts.
/// </summary>
public static class SavingsAccountEndpoints
{
    /// <summary>
    /// Maps all Savings Account endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapSavingsAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var savingsAccountsGroup = app.MapGroup("savings-accounts").WithTags("savings-accounts");

        savingsAccountsGroup.MapPost("/", async (CreateSavingsAccountCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/savings-accounts/{response.Id}", response);
            })
            .WithName("CreateSavingsAccount")
            .WithSummary("Creates a new savings account")
            .Produces<CreateSavingsAccountResponse>(StatusCodes.Status201Created);

        savingsAccountsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetSavingsAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetSavingsAccount")
            .WithSummary("Gets a savings account by ID")
            .Produces<SavingsAccountResponse>();

        savingsAccountsGroup.MapPost("/search", async (SearchSavingsAccountsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchSavingsAccounts")
            .WithSummary("Searches savings accounts with filters and pagination")
            .Produces<PagedList<SavingsAccountResponse>>();

        savingsAccountsGroup.MapPost("/{id:guid}/deposit", async (Guid id, DepositCommand command, ISender sender) =>
            {
                if (id != command.AccountId)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DepositToSavingsAccount")
            .WithSummary("Deposits money to a savings account")
            .Produces<DepositResponse>();

        savingsAccountsGroup.MapPost("/{id:guid}/withdraw", async (Guid id, WithdrawCommand command, ISender sender) =>
            {
                if (id != command.AccountId)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("WithdrawFromSavingsAccount")
            .WithSummary("Withdraws money from a savings account")
            .Produces<WithdrawResponse>();

        savingsAccountsGroup.MapPost("/transfer", async (TransferFundsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("TransferFunds")
            .WithSummary("Transfers funds between savings accounts")
            .Produces<TransferFundsResponse>();

        savingsAccountsGroup.MapGet("/by-member/{memberId:guid}", async (Guid memberId, ISender sender) =>
            {
                var command = new SearchSavingsAccountsCommand
                {
                    MemberId = memberId,
                    PageNumber = 1,
                    PageSize = 100
                };
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetSavingsAccountsByMember")
            .WithSummary("Gets all savings accounts for a member")
            .Produces<PagedList<SavingsAccountResponse>>();

        return app;
    }
}

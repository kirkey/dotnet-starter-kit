using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;
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

        return app;
    }
}

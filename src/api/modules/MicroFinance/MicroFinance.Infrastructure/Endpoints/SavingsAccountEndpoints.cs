using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Freeze.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.PostInterest.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Transfer.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Unfreeze.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Withdraw.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Savings Accounts.
/// </summary>
public class SavingsAccountEndpoints : CarterModule
{

    private const string ActivateSavingsAccount = "ActivateSavingsAccount";
    private const string CloseSavingsAccount = "CloseSavingsAccount";
    private const string CreateSavingsAccount = "CreateSavingsAccount";
    private const string DepositToSavingsAccount = "DepositToSavingsAccount";
    private const string FreezeSavingsAccount = "FreezeSavingsAccount";
    private const string GetSavingsAccount = "GetSavingsAccount";
    private const string GetSavingsAccountsByMember = "GetSavingsAccountsByMember";
    private const string PostSavingsInterest = "PostSavingsInterest";
    private const string SearchSavingsAccounts = "SearchSavingsAccounts";
    private const string TransferFunds = "TransferFunds";
    private const string UnfreezeSavingsAccount = "UnfreezeSavingsAccount";
    private const string WithdrawFromSavingsAccount = "WithdrawFromSavingsAccount";

    /// <summary>
    /// Maps all Savings Account endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var savingsAccountsGroup = app.MapGroup("microfinance/savings-accounts").WithTags("Savings Accounts");

        savingsAccountsGroup.MapPost("/", async (CreateSavingsAccountCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/savings-accounts/{response.Id}", response);
            })
            .WithName(CreateSavingsAccount)
            .WithSummary("Creates a new savings account")
            .Produces<CreateSavingsAccountResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetSavingsAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetSavingsAccount)
            .WithSummary("Gets a savings account by ID")
            .Produces<SavingsAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/search", async (SearchSavingsAccountsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchSavingsAccounts)
            .WithSummary("Searches savings accounts with filters and pagination")
            .Produces<PagedList<SavingsAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/{id}/deposit", async (DefaultIdType id, DepositCommand command, ISender sender) =>
            {
                if (id != command.AccountId)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DepositToSavingsAccount)
            .WithSummary("Deposits money to a savings account")
            .Produces<DepositResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Deposit, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/{id}/withdraw", async (DefaultIdType id, WithdrawCommand command, ISender sender) =>
            {
                if (id != command.AccountId)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(WithdrawFromSavingsAccount)
            .WithSummary("Withdraws money from a savings account")
            .Produces<WithdrawResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Withdraw, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/transfer", async (TransferFundsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(TransferFunds)
            .WithSummary("Transfers funds between savings accounts")
            .Produces<TransferFundsResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Transfer, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapGet("/by-member/{memberId:guid}", async (DefaultIdType memberId, ISender sender) =>
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
            .WithName(GetSavingsAccountsByMember)
            .WithSummary("Gets all savings accounts for a member")
            .Produces<PagedList<SavingsAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/{id}/post-interest", async (DefaultIdType id, PostInterestCommand command, ISender sender) =>
            {
                if (id != command.AccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(PostSavingsInterest)
            .WithSummary("Posts interest to a savings account")
            .Produces<PostInterestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/{id}/freeze", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new FreezeAccountCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(FreezeSavingsAccount)
            .WithSummary("Freezes a savings account")
            .Produces<FreezeAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Freeze, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/{id}/unfreeze", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new UnfreezeAccountCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UnfreezeSavingsAccount)
            .WithSummary("Unfreezes a frozen savings account")
            .Produces<UnfreezeAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Unfreeze, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/{id}/close", async (DefaultIdType id, CloseAccountCommand command, ISender sender) =>
            {
                if (id != command.AccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CloseSavingsAccount)
            .WithSummary("Closes a savings account")
            .Produces<CloseAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
            .MapToApiVersion(1);

        savingsAccountsGroup.MapPost("/{id}/activate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateSavingsAccountCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivateSavingsAccount)
            .WithSummary("Activates a pending savings account")
            .Produces<ActivateSavingsAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Activate, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}

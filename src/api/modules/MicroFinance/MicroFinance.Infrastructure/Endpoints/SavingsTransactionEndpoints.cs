using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Savings Transactions.
/// </summary>
public class SavingsTransactionEndpoints() : CarterModule
{

    private const string GetSavingsTransaction = "GetSavingsTransaction";
    private const string GetSavingsTransactionsByAccount = "GetSavingsTransactionsByAccount";
    private const string SearchSavingsTransactions = "SearchSavingsTransactions";

    /// <summary>
    /// Maps all Savings Transaction endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var transactionsGroup = app.MapGroup("microfinance/savings-transactions").WithTags("Savings Transactions");

        transactionsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetSavingsTransactionRequest(id));
            return Results.Ok(response);
        })
        .WithName(GetSavingsTransaction)
        .WithSummary("Gets a savings transaction by ID")
        .Produces<SavingsTransactionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        transactionsGroup.MapPost("/search", async (SearchSavingsTransactionsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(SearchSavingsTransactions)
        .WithSummary("Searches savings transactions with pagination")
        .Produces<PagedList<SavingsTransactionResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        transactionsGroup.MapGet("/by-account/{accountId:guid}", async (Guid accountId, ISender mediator) =>
        {
            var request = new SearchSavingsTransactionsCommand
            {
                SavingsAccountId = accountId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(GetSavingsTransactionsByAccount)
        .WithSummary("Gets all transactions for a savings account")
        .Produces<PagedList<SavingsTransactionResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

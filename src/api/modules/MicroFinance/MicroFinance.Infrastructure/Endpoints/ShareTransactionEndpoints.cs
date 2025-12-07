using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Share Transactions.
/// </summary>
public class ShareTransactionEndpoints() : CarterModule
{

    private const string GetShareTransaction = "GetShareTransaction";
    private const string GetShareTransactionsByAccount = "GetShareTransactionsByAccount";
    private const string SearchShareTransactions = "SearchShareTransactions";

    /// <summary>
    /// Maps all Share Transaction endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var transactionsGroup = app.MapGroup("microfinance/share-transactions").WithTags("share-transactions");

        transactionsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetShareTransactionRequest(id));
            return Results.Ok(response);
        })
        .WithName(GetShareTransaction)
        .WithSummary("Gets a share transaction by ID")
        .Produces<ShareTransactionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        transactionsGroup.MapPost("/search", async (SearchShareTransactionsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(SearchShareTransactions)
        .WithSummary("Searches share transactions with pagination")
        .Produces<PagedList<ShareTransactionResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        transactionsGroup.MapGet("/by-account/{accountId:guid}", async (Guid accountId, ISender mediator) =>
        {
            var request = new SearchShareTransactionsCommand
            {
                ShareAccountId = accountId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(GetShareTransactionsByAccount)
        .WithSummary("Gets all transactions for a share account")
        .Produces<PagedList<ShareTransactionResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

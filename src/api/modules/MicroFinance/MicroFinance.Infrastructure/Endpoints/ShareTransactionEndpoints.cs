using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Search.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Share Transactions.
/// </summary>
public static class ShareTransactionEndpoints
{
    /// <summary>
    /// Maps all Share Transaction endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapShareTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var transactionsGroup = app.MapGroup("share-transactions").WithTags("share-transactions");

        transactionsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetShareTransactionRequest(id));
            return Results.Ok(response);
        })
        .WithName("GetShareTransaction")
        .WithSummary("Gets a share transaction by ID")
        .Produces<ShareTransactionResponse>();

        transactionsGroup.MapPost("/search", async (SearchShareTransactionsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("SearchShareTransactions")
        .WithSummary("Searches share transactions with pagination")
        .Produces<PagedList<ShareTransactionResponse>>();

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
        .WithName("GetShareTransactionsByAccount")
        .WithSummary("Gets all transactions for a share account")
        .Produces<PagedList<ShareTransactionResponse>>();

        return app;
    }
}

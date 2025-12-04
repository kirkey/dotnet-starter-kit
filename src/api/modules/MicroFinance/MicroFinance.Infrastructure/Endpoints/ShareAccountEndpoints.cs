using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PurchaseShares.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Share Accounts.
/// </summary>
public static class ShareAccountEndpoints
{
    /// <summary>
    /// Maps all Share Account endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapShareAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var shareAccountsGroup = app.MapGroup("share-accounts").WithTags("share-accounts");

        shareAccountsGroup.MapPost("/", async (CreateShareAccountCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/share-accounts/{response.Id}", response);
            })
            .WithName("CreateShareAccount")
            .WithSummary("Creates a new share account")
            .Produces<CreateShareAccountResponse>(StatusCodes.Status201Created);

        shareAccountsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetShareAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetShareAccount")
            .WithSummary("Gets a share account by ID")
            .Produces<ShareAccountResponse>();

        shareAccountsGroup.MapPost("/search", async (SearchShareAccountsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchShareAccounts")
            .WithSummary("Searches share accounts with filtering and pagination")
            .Produces<PagedList<ShareAccountResponse>>();

        shareAccountsGroup.MapPost("/{id:guid}/purchase", async (Guid id, PurchaseSharesCommand command, ISender sender) =>
            {
                if (id != command.ShareAccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("PurchaseShares")
            .WithSummary("Purchases shares for an account")
            .Produces<PurchaseSharesResponse>();

        return app;
    }
}

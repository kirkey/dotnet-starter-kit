using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PayDividend.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PostDividend.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PurchaseShares.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.RedeemShares.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Share Accounts.
/// </summary>
public class ShareAccountEndpoints : CarterModule
{

    private const string ActivateShareAccount = "ActivateShareAccount";
    private const string ApproveShareAccount = "ApproveShareAccount";
    private const string CloseShareAccount = "CloseShareAccount";
    private const string CreateShareAccount = "CreateShareAccount";
    private const string GetShareAccount = "GetShareAccount";
    private const string PayShareDividend = "PayShareDividend";
    private const string PostShareDividend = "PostShareDividend";
    private const string PurchaseShares = "PurchaseShares";
    private const string RedeemShares = "RedeemShares";
    private const string SearchShareAccounts = "SearchShareAccounts";

    /// <summary>
    /// Maps all Share Account endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var shareAccountsGroup = app.MapGroup("microfinance/share-accounts").WithTags("Share Accounts");

        shareAccountsGroup.MapPost("/", async (CreateShareAccountCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/share-accounts/{response.Id}", response);
            })
            .WithName(CreateShareAccount)
            .WithSummary("Creates a new share account")
            .Produces<CreateShareAccountResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetShareAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetShareAccount)
            .WithSummary("Gets a share account by ID")
            .Produces<ShareAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapPost("/search", async (SearchShareAccountsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchShareAccounts)
            .WithSummary("Searches share accounts with filtering and pagination")
            .Produces<PagedList<ShareAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapPost("/{id}/purchase", async (DefaultIdType id, PurchaseSharesCommand command, ISender sender) =>
            {
                if (id != command.ShareAccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(PurchaseShares)
            .WithSummary("Purchases shares for an account")
            .Produces<PurchaseSharesResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapPost("/{id}/redeem", async (DefaultIdType id, RedeemSharesCommand command, ISender sender) =>
            {
                if (id != command.ShareAccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RedeemShares)
            .WithSummary("Redeems shares from an account")
            .Produces<RedeemSharesResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapPost("/{id}/post-dividend", async (DefaultIdType id, PostDividendCommand command, ISender sender) =>
            {
                if (id != command.ShareAccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(PostShareDividend)
            .WithSummary("Posts a dividend to a share account")
            .Produces<PostDividendResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapPost("/{id}/pay-dividend", async (DefaultIdType id, PayDividendCommand command, ISender sender) =>
            {
                if (id != command.ShareAccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(PayShareDividend)
            .WithSummary("Pays out the dividend on a share account")
            .Produces<PayDividendResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapPost("/{id}/close", async (DefaultIdType id, CloseShareAccountCommand command, ISender sender) =>
            {
                if (id != command.AccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CloseShareAccount)
            .WithSummary("Closes a share account")
            .Produces<CloseShareAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapPost("/{id}/approve", async (DefaultIdType id, ApproveShareAccountCommand command, ISender sender) =>
            {
                if (id != command.ShareAccountId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ApproveShareAccount)
            .WithSummary("Approves a pending share account")
            .Produces<ApproveShareAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
            .MapToApiVersion(1);

        shareAccountsGroup.MapPost("/{id}/activate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateShareAccountCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivateShareAccount)
            .WithSummary("Activates an approved share account")
            .Produces<ActivateShareAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Activate, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}

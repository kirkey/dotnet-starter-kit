using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Credit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Debit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.LinkSavings.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Suspend.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.UpgradeTier.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class MobileWalletEndpoints() : CarterModule
{

    private const string ActivateMobileWallet = "ActivateMobileWallet";
    private const string CreateMobileWallet = "CreateMobileWallet";
    private const string CreditMobileWallet = "CreditMobileWallet";
    private const string DebitMobileWallet = "DebitMobileWallet";
    private const string GetMobileWallet = "GetMobileWallet";
    private const string LinkSavingsMobileWallet = "LinkSavingsMobileWallet";
    private const string SearchMobileWallets = "SearchMobileWallets";
    private const string SuspendMobileWallet = "SuspendMobileWallet";
    private const string UpgradeTierMobileWallet = "UpgradeTierMobileWallet";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/mobile-wallets").WithTags("Mobile Wallets");

        group.MapPost("/", async (CreateMobileWalletCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/mobile-wallets/{result.Id}", result);
        })
        .WithName(CreateMobileWallet)
        .WithSummary("Create a new mobile wallet")
        .Produces<CreateMobileWalletResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetMobileWalletRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetMobileWallet)
        .WithSummary("Get mobile wallet by ID")
        .Produces<MobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/credit", async (Guid id, CreditRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreditMobileWalletCommand(id, request.Amount, request.TransactionReference));
            return Results.Ok(result);
        })
        .WithName(CreditMobileWallet)
        .WithSummary("Credit mobile wallet")
        .Produces<CreditMobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Deposit, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/debit", async (Guid id, DebitRequest request, ISender sender) =>
        {
            var result = await sender.Send(new DebitMobileWalletCommand(id, request.Amount, request.TransactionReference)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(DebitMobileWallet)
        .WithSummary("Debit mobile wallet")
        .Produces<DebitMobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Withdraw, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateMobileWalletCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ActivateMobileWallet)
        .WithSummary("Activate a mobile wallet")
        .Produces<ActivateMobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/suspend", async (Guid id, SuspendMobileWalletRequest request, ISender sender) =>
        {
            var result = await sender.Send(new SuspendMobileWalletCommand(id, request.Reason)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SuspendMobileWallet)
        .WithSummary("Suspend a mobile wallet")
        .Produces<SuspendMobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Cancel, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/link-savings", async (Guid id, LinkSavingsRequest request, ISender sender) =>
        {
            var result = await sender.Send(new LinkSavingsMobileWalletCommand(id, request.SavingsAccountId)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(LinkSavingsMobileWallet)
        .WithSummary("Link mobile wallet to a savings account")
        .Produces<LinkSavingsMobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/upgrade-tier", async (Guid id, UpgradeTierRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpgradeTierMobileWalletCommand(id, request.NewTier, request.NewDailyLimit, request.NewMonthlyLimit)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpgradeTierMobileWallet)
        .WithSummary("Upgrade mobile wallet tier")
        .Produces<UpgradeTierMobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchMobileWalletsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchMobileWallets)
        .WithSummary("Search mobile wallets with filters and pagination")
        .Produces<PagedList<MobileWalletResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record CreditRequest(decimal Amount, string TransactionReference);
public record DebitRequest(decimal Amount, string TransactionReference);
public record SuspendMobileWalletRequest(string Reason);
public record LinkSavingsRequest(Guid SavingsAccountId);
public record UpgradeTierRequest(string NewTier, decimal NewDailyLimit, decimal NewMonthlyLimit);

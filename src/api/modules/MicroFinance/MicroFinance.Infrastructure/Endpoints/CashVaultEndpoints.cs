using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.CloseDay.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Deposit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.OpenDay.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Reconcile.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.TransferToVault.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Withdraw.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CashVaultEndpoints : CarterModule
{

    private const string CloseDayVault = "CloseDayVault";
    private const string CreateCashVault = "CreateCashVault";
    private const string DepositCash = "DepositCash";
    private const string GetCashVault = "GetCashVault";
    private const string OpenVaultDay = "OpenVaultDay";
    private const string ReconcileVault = "ReconcileVault";
    private const string SearchCashVaults = "SearchCashVaults";
    private const string TransferBetweenVaults = "TransferBetweenVaults";
    private const string WithdrawCash = "WithdrawCash";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/cash-vaults").WithTags("Cash Vaults");

        // CRUD Operations
        group.MapPost("/", async (CreateCashVaultCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/cash-vaults/{response.Id}", response);
            })
            .WithName(CreateCashVault)
            .WithSummary("Creates a new cash vault")
            .Produces<CreateCashVaultResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetCashVaultRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetCashVault)
            .WithSummary("Gets a cash vault by ID")
            .Produces<CashVaultResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Cash Operations
        group.MapPost("/{id:guid}/deposit", async (DefaultIdType id, DepositCashCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DepositCash)
            .WithSummary("Deposits cash into the vault")
            .Produces<DepositCashResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Deposit, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/withdraw", async (DefaultIdType id, WithdrawCashCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(WithdrawCash)
            .WithSummary("Withdraws cash from the vault")
            .Produces<WithdrawCashResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Withdraw, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Day Operations
        group.MapPost("/{id:guid}/open-day", async (DefaultIdType id, OpenDayCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(OpenVaultDay)
            .WithSummary("Opens a new business day for the vault")
            .Produces<OpenDayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reconcile", async (DefaultIdType id, ReconcileCashVaultCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ReconcileVault)
            .WithSummary("Performs end-of-day reconciliation")
            .Produces<ReconcileCashVaultResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/close-day", async (DefaultIdType id, CloseDayCashVaultCommand command, ISender sender) =>
            {
                if (id != command.CashVaultId) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CloseDayVault)
            .WithSummary("Closes the vault for the business day")
            .Produces<CloseDayCashVaultResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/transfer", async (TransferToVaultCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(TransferBetweenVaults)
            .WithSummary("Transfers cash between vaults")
            .Produces<TransferToVaultResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Transfer, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCashVaultsCommand command, ISender sender) =>
            {
                var result = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(SearchCashVaults)
            .WithSummary("Search cash vaults")
            .Produces<PagedList<CashVaultSummaryResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}

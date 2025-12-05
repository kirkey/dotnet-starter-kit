using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Deposit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.OpenDay.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Reconcile.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Withdraw.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CashVaultEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/cash-vaults").WithTags("cash-vaults");

        // CRUD Operations
        group.MapPost("/", async (CreateCashVaultCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/cash-vaults/{response.Id}", response);
            })
            .WithName("CreateCashVault")
            .WithSummary("Creates a new cash vault")
            .Produces<CreateCashVaultResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetCashVaultRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetCashVault")
            .WithSummary("Gets a cash vault by ID")
            .Produces<CashVaultResponse>();

        // Cash Operations
        group.MapPost("/{id:guid}/deposit", async (Guid id, DepositCashCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DepositCash")
            .WithSummary("Deposits cash into the vault")
            .Produces<DepositCashResponse>();

        group.MapPost("/{id:guid}/withdraw", async (Guid id, WithdrawCashCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("WithdrawCash")
            .WithSummary("Withdraws cash from the vault")
            .Produces<WithdrawCashResponse>();

        // Day Operations
        group.MapPost("/{id:guid}/open-day", async (Guid id, OpenDayCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("OpenVaultDay")
            .WithSummary("Opens a new business day for the vault")
            .Produces<OpenDayResponse>();

        group.MapPost("/{id:guid}/reconcile", async (Guid id, ReconcileCashVaultCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ReconcileVault")
            .WithSummary("Performs end-of-day reconciliation")
            .Produces<ReconcileCashVaultResponse>();

    }
}

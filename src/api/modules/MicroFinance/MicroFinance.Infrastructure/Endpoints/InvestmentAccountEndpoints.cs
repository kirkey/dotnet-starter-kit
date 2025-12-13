using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Invest.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Redeem.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.SetupSip.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InvestmentAccountEndpoints : CarterModule
{
    private const string CreateInvestmentAccount = "CreateInvestmentAccount";
    private const string GetInvestmentAccount = "GetInvestmentAccount";
    private const string InvestInAccount = "InvestInAccount";
    private const string RedeemFromAccount = "RedeemFromAccount";
    private const string SearchInvestmentAccounts = "SearchInvestmentAccounts";
    private const string SetupSip = "SetupSip";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/investment-accounts").WithTags("Investment Accounts");

        // Search accounts
        group.MapPost("/search", async (SearchInvestmentAccountsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(SearchInvestmentAccounts)
        .WithSummary("Search investment accounts")
        .Produces<PagedList<InvestmentAccountResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/", async (CreateInvestmentAccountCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/investment-accounts/{result.Id}", result);
        })
        .WithName(CreateInvestmentAccount)
        .WithSummary("Create a new investment account")
        .Produces<CreateInvestmentAccountResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetInvestmentAccountRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetInvestmentAccount)
        .WithSummary("Get investment account by ID")
        .Produces<InvestmentAccountResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/invest", async (DefaultIdType id, InvestAccountRequest request, ISender sender) =>
        {
            var result = await sender.Send(new InvestCommand(id, request.Amount));
            return Results.Ok(result);
        })
        .WithName(InvestInAccount)
        .WithSummary("Add investment to account")
        .Produces<InvestResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/redeem", async (DefaultIdType id, RedeemAccountRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RedeemCommand(id, request.Amount, request.GainLoss));
            return Results.Ok(result);
        })
        .WithName(RedeemFromAccount)
        .WithSummary("Redeem from investment account")
        .Produces<RedeemResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/sip", async (DefaultIdType id, SetupSipRequest request, ISender sender) =>
        {
            var result = await sender.Send(new SetupSipCommand(id, request.Amount, request.Frequency, request.NextDate, request.LinkedSavingsAccountId));
            return Results.Ok(result);
        })
        .WithName(SetupSip)
        .WithSummary("Setup SIP for investment account")
        .Produces<SetupSipResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record InvestAccountRequest(decimal Amount);
public record RedeemAccountRequest(decimal Amount, decimal GainLoss);
public record SetupSipRequest(decimal Amount, string Frequency, DateOnly NextDate, DefaultIdType LinkedSavingsAccountId);

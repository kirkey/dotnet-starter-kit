using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Invest.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Redeem.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.SetupSip.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InvestmentAccountEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/investment-accounts").WithTags("Investment Accounts");

        group.MapPost("/", async (CreateInvestmentAccountCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/investment-accounts/{result.Id}", result);
        })
        .WithName("CreateInvestmentAccount")
        .WithSummary("Create a new investment account")
        .Produces<CreateInvestmentAccountResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetInvestmentAccountRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetInvestmentAccount")
        .WithSummary("Get investment account by ID")
        .Produces<InvestmentAccountResponse>();

        group.MapPost("/{id:guid}/invest", async (Guid id, InvestAccountRequest request, ISender sender) =>
        {
            var result = await sender.Send(new InvestCommand(id, request.Amount));
            return Results.Ok(result);
        })
        .WithName("InvestInAccount")
        .WithSummary("Add investment to account")
        .Produces<InvestResponse>();

        group.MapPost("/{id:guid}/redeem", async (Guid id, RedeemAccountRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RedeemCommand(id, request.Amount, request.GainLoss));
            return Results.Ok(result);
        })
        .WithName("RedeemFromAccount")
        .WithSummary("Redeem from investment account")
        .Produces<RedeemResponse>();

        group.MapPost("/{id:guid}/sip", async (Guid id, SetupSipRequest request, ISender sender) =>
        {
            var result = await sender.Send(new SetupSipCommand(id, request.Amount, request.Frequency, request.NextDate, request.LinkedSavingsAccountId));
            return Results.Ok(result);
        })
        .WithName("SetupSip")
        .WithSummary("Setup SIP for investment account")
        .Produces<SetupSipResponse>();

    }
}

public record InvestAccountRequest(decimal Amount);
public record RedeemAccountRequest(decimal Amount, decimal GainLoss);
public record SetupSipRequest(decimal Amount, string Frequency, DateOnly NextDate, Guid LinkedSavingsAccountId);

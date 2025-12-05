using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Complete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateBuy.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateSell.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InvestmentTransactionEndpoints() : CarterModule("microfinance")
{

    private const string CompleteInvestmentTransaction = "CompleteInvestmentTransaction";
    private const string CreateBuyTransaction = "CreateBuyTransaction";
    private const string CreateSellTransaction = "CreateSellTransaction";
    private const string GetInvestmentTransaction = "GetInvestmentTransaction";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/investment-transactions").WithTags("Investment Transactions");

        group.MapPost("/buy", async (CreateBuyTransactionCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/investment-transactions/{result.Id}", result);
        })
        .WithName(CreateBuyTransaction)
        .WithSummary("Create a buy investment transaction")
        .Produces<CreateBuyTransactionResponse>(StatusCodes.Status201Created);

        group.MapPost("/sell", async (CreateSellTransactionCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/investment-transactions/{result.Id}", result);
        })
        .WithName(CreateSellTransaction)
        .WithSummary("Create a sell investment transaction")
        .Produces<CreateSellTransactionResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetInvestmentTransactionRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetInvestmentTransaction)
        .WithSummary("Get investment transaction by ID")
        .Produces<InvestmentTransactionResponse>();

        group.MapPost("/{id:guid}/complete", async (Guid id, CompleteTransactionRequest? request, ISender sender) =>
        {
            var result = await sender.Send(new CompleteTransactionCommand(id, request?.GainLoss));
            return Results.Ok(result);
        })
        .WithName(CompleteInvestmentTransaction)
        .WithSummary("Complete an investment transaction")
        .Produces<CompleteTransactionResponse>();

    }
}

public record CompleteTransactionRequest(decimal? GainLoss);

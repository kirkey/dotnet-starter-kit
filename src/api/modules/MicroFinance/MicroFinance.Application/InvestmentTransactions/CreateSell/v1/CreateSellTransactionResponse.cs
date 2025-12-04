namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateSell.v1;

public sealed record CreateSellTransactionResponse(
    Guid Id,
    string TransactionReference,
    string Status,
    decimal? Units);

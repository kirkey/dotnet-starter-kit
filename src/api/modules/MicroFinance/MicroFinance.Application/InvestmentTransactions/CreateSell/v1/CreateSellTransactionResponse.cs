namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateSell.v1;

public sealed record CreateSellTransactionResponse(
    DefaultIdType Id,
    string TransactionReference,
    string Status,
    decimal? Units);

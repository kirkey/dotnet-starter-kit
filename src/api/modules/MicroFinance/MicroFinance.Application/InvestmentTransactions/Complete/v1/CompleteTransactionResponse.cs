namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Complete.v1;

public sealed record CompleteTransactionResponse(
    DefaultIdType Id,
    string Status,
    decimal NetAmount,
    decimal? GainLoss);

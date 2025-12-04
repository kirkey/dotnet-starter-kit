namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Complete.v1;

public sealed record CompleteTransactionResponse(
    Guid Id,
    string Status,
    decimal NetAmount,
    decimal? GainLoss);

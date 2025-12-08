namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateBuy.v1;

public sealed record CreateBuyTransactionResponse(
    DefaultIdType Id,
    string TransactionReference,
    string Status,
    decimal Amount,
    decimal NetAmount);

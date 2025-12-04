namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateBuy.v1;

public sealed record CreateBuyTransactionResponse(
    Guid Id,
    string TransactionReference,
    string Status,
    decimal Amount,
    decimal NetAmount);

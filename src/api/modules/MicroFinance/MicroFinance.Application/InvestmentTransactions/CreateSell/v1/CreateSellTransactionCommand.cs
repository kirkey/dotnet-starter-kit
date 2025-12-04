using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateSell.v1;

public sealed record CreateSellTransactionCommand(
    Guid InvestmentAccountId,
    Guid ProductId,
    string TransactionReference,
    decimal Units,
    decimal? ExitLoad = null) : IRequest<CreateSellTransactionResponse>;

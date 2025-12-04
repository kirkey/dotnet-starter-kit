using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.CreateBuy.v1;

public sealed record CreateBuyTransactionCommand(
    Guid InvestmentAccountId,
    Guid ProductId,
    string TransactionReference,
    decimal Amount,
    decimal? EntryLoad = null,
    string? PaymentMode = null,
    string? PaymentReference = null) : IRequest<CreateBuyTransactionResponse>;

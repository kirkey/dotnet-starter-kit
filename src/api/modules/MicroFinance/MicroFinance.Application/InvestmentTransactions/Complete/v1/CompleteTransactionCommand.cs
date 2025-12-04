using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Complete.v1;

public sealed record CompleteTransactionCommand(
    Guid Id,
    decimal? GainLoss = null) : IRequest<CompleteTransactionResponse>;

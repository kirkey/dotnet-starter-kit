using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Complete.v1;

public sealed record CompleteMobileTransactionCommand(
    DefaultIdType Id,
    string ProviderResponse) : IRequest<CompleteMobileTransactionResponse>;

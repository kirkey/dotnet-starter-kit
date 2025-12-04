using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Complete.v1;

public sealed record CompleteMobileTransactionCommand(
    Guid Id,
    string ProviderResponse) : IRequest<CompleteMobileTransactionResponse>;

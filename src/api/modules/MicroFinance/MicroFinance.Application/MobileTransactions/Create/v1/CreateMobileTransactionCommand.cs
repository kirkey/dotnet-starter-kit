using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Create.v1;

public sealed record CreateMobileTransactionCommand(
    DefaultIdType WalletId,
    string TransactionReference,
    string TransactionType,
    decimal Amount,
    decimal Fee,
    string? SourcePhone = null,
    string? DestinationPhone = null) : IRequest<CreateMobileTransactionResponse>;

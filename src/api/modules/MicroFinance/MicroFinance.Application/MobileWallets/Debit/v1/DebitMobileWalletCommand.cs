using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Debit.v1;

public sealed record DebitMobileWalletCommand(
    Guid Id,
    decimal Amount,
    string TransactionReference) : IRequest<DebitMobileWalletResponse>;

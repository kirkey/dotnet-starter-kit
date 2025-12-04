using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Credit.v1;

public sealed record CreditMobileWalletCommand(
    Guid Id,
    decimal Amount,
    string TransactionReference) : IRequest<CreditMobileWalletResponse>;

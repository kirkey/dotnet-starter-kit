using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.LinkSavings.v1;

public sealed record LinkSavingsMobileWalletCommand(DefaultIdType Id, DefaultIdType SavingsAccountId) : IRequest<LinkSavingsMobileWalletResponse>;

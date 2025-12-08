namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Debit.v1;

public sealed record DebitMobileWalletResponse(
    DefaultIdType Id,
    decimal Balance);

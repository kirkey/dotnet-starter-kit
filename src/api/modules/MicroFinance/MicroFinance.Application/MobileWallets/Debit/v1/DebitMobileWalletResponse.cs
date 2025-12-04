namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Debit.v1;

public sealed record DebitMobileWalletResponse(
    Guid Id,
    decimal Balance);

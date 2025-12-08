using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Create.v1;

public sealed record CreateMobileWalletCommand(
    DefaultIdType MemberId,
    string PhoneNumber,
    string Provider,
    decimal DailyLimit,
    decimal MonthlyLimit) : IRequest<CreateMobileWalletResponse>;

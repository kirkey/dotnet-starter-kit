using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.CreateDynamic.v1;

public sealed record CreateDynamicQrCommand(
    string QrCode,
    decimal Amount,
    string Reference,
    DateTimeOffset ExpiresAt,
    Guid? WalletId = null,
    Guid? MemberId = null) : IRequest<CreateDynamicQrResponse>;

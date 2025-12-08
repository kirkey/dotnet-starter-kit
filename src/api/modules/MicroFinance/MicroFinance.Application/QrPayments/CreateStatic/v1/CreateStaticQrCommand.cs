using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.CreateStatic.v1;

public sealed record CreateStaticQrCommand(
    string QrCode,
    DefaultIdType? WalletId = null,
    DefaultIdType? MemberId = null,
    DefaultIdType? AgentId = null) : IRequest<CreateStaticQrResponse>;

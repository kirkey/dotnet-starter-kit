using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;

public sealed class GetQrPaymentHandler(
    [FromKeyedServices("microfinance:qrpayments")] IReadRepository<QrPayment> repository)
    : IRequestHandler<GetQrPaymentRequest, QrPaymentResponse>
{
    public async Task<QrPaymentResponse> Handle(GetQrPaymentRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var qr = await repository.FirstOrDefaultAsync(new QrPaymentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"QR payment with id {request.Id} not found");

        return new QrPaymentResponse(
            qr.Id,
            qr.WalletId,
            qr.MemberId,
            qr.AgentId,
            qr.QrCode,
            qr.QrType,
            qr.Status,
            qr.Amount,
            qr.Reference,
            qr.Notes,
            qr.MaxUses,
            qr.CurrentUses,
            qr.GeneratedAt,
            qr.ExpiresAt,
            qr.LastUsedAt,
            qr.LastTransactionId);
    }
}

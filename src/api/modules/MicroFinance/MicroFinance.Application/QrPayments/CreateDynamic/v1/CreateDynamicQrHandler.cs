using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.CreateDynamic.v1;

public sealed class CreateDynamicQrHandler(
    ILogger<CreateDynamicQrHandler> logger,
    [FromKeyedServices("microfinance:qrpayments")] IRepository<QrPayment> repository)
    : IRequestHandler<CreateDynamicQrCommand, CreateDynamicQrResponse>
{
    public async Task<CreateDynamicQrResponse> Handle(CreateDynamicQrCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var qr = QrPayment.CreateDynamic(
            request.QrCode,
            request.Amount,
            request.Reference,
            request.ExpiresAt,
            request.WalletId,
            request.MemberId);

        await repository.AddAsync(qr, cancellationToken);
        logger.LogInformation("Dynamic QR payment created with ID {Id}", qr.Id);

        return new CreateDynamicQrResponse(qr.Id, qr.QrCode, qr.ExpiresAt!.Value);
    }
}

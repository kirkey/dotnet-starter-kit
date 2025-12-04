using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.CreateStatic.v1;

public sealed class CreateStaticQrHandler(
    ILogger<CreateStaticQrHandler> logger,
    [FromKeyedServices("microfinance:qrpayments")] IRepository<QrPayment> repository)
    : IRequestHandler<CreateStaticQrCommand, CreateStaticQrResponse>
{
    public async Task<CreateStaticQrResponse> Handle(CreateStaticQrCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var qr = QrPayment.CreateStatic(
            request.QrCode,
            request.WalletId,
            request.MemberId,
            request.AgentId);

        await repository.AddAsync(qr, cancellationToken);
        logger.LogInformation("Static QR payment created with ID {Id}", qr.Id);

        return new CreateStaticQrResponse(qr.Id, qr.QrCode);
    }
}

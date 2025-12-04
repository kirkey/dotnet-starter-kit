using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Activate.v1;

public sealed class ActivatePaymentGatewayHandler(
    ILogger<ActivatePaymentGatewayHandler> logger,
    [FromKeyedServices("microfinance:paymentgateways")] IRepository<PaymentGateway> repository)
    : IRequestHandler<ActivatePaymentGatewayCommand, ActivatePaymentGatewayResponse>
{
    public async Task<ActivatePaymentGatewayResponse> Handle(ActivatePaymentGatewayCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var gateway = await repository.FirstOrDefaultAsync(new PaymentGatewayByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Payment gateway with id {request.Id} not found");

        gateway.Activate();
        await repository.UpdateAsync(gateway, cancellationToken);

        logger.LogInformation("Payment gateway {Id} activated", gateway.Id);
        return new ActivatePaymentGatewayResponse(gateway.Id, gateway.Status);
    }
}

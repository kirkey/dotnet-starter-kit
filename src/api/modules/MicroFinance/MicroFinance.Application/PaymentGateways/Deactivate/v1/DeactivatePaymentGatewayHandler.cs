using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Deactivate.v1;

/// <summary>
/// Handler for deactivating a payment gateway.
/// </summary>
public sealed class DeactivatePaymentGatewayHandler(
    ILogger<DeactivatePaymentGatewayHandler> logger,
    [FromKeyedServices("microfinance:paymentgateways")] IRepository<PaymentGateway> repository)
    : IRequestHandler<DeactivatePaymentGatewayCommand, DeactivatePaymentGatewayResponse>
{
    /// <summary>
    /// Handles the deactivate payment gateway command.
    /// </summary>
    public async Task<DeactivatePaymentGatewayResponse> Handle(
        DeactivatePaymentGatewayCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var gateway = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"Payment gateway with ID {request.Id} not found.");

        gateway.Deactivate();

        await repository.UpdateAsync(gateway, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Payment gateway {Provider} deactivated", gateway.Provider);

        return new DeactivatePaymentGatewayResponse(
            gateway.Id,
            gateway.Provider,
            gateway.Status);
    }
}

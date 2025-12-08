using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Update.v1;

/// <summary>
/// Handler for updating a payment gateway.
/// </summary>
public sealed class UpdatePaymentGatewayHandler(
    ILogger<UpdatePaymentGatewayHandler> logger,
    [FromKeyedServices("microfinance:paymentgateways")] IRepository<PaymentGateway> repository)
    : IRequestHandler<UpdatePaymentGatewayCommand, UpdatePaymentGatewayResponse>
{
    /// <summary>
    /// Handles the update payment gateway command.
    /// </summary>
    public async Task<UpdatePaymentGatewayResponse> Handle(
        UpdatePaymentGatewayCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var gateway = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"Payment gateway with ID {request.Id} not found.");

        gateway.Update(
            request.Name,
            request.TransactionFeePercent,
            request.TransactionFeeFixed,
            request.MinTransactionAmount,
            request.MaxTransactionAmount,
            request.SupportsRefunds,
            request.SupportsRecurring,
            request.SupportsMobileWallet,
            request.SupportsCardPayments,
            request.SupportsBankTransfer,
            request.TimeoutSeconds,
            request.RetryAttempts);

        await repository.UpdateAsync(gateway, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Payment gateway {Provider} updated", gateway.Provider);

        return new UpdatePaymentGatewayResponse(
            gateway.Id,
            gateway.Provider,
            gateway.Status,
            gateway.TransactionFeePercent,
            gateway.TransactionFeeFixed,
            gateway.MinTransactionAmount,
            gateway.MaxTransactionAmount);
    }
}

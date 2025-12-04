using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;

public sealed class GetPaymentGatewayHandler(
    [FromKeyedServices("microfinance:paymentgateways")] IReadRepository<PaymentGateway> repository)
    : IRequestHandler<GetPaymentGatewayRequest, PaymentGatewayResponse>
{
    public async Task<PaymentGatewayResponse> Handle(GetPaymentGatewayRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var gateway = await repository.FirstOrDefaultAsync(new PaymentGatewayByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Payment gateway with id {request.Id} not found");

        return new PaymentGatewayResponse(
            gateway.Id,
            gateway.Name,
            gateway.Provider,
            gateway.Status,
            gateway.MerchantId,
            gateway.WebhookUrl,
            gateway.TransactionFeePercent,
            gateway.TransactionFeeFixed,
            gateway.MinTransactionAmount,
            gateway.MaxTransactionAmount,
            gateway.SupportsRefunds,
            gateway.SupportsRecurring,
            gateway.SupportsMobileWallet,
            gateway.SupportsCardPayments,
            gateway.SupportsBankTransfer,
            gateway.IsTestMode,
            gateway.TimeoutSeconds,
            gateway.RetryAttempts,
            gateway.LastSuccessfulConnection,
            gateway.CreatedOn);
    }
}

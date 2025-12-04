using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Create.v1;

public sealed class CreatePaymentGatewayHandler(
    ILogger<CreatePaymentGatewayHandler> logger,
    [FromKeyedServices("microfinance:paymentgateways")] IRepository<PaymentGateway> repository)
    : IRequestHandler<CreatePaymentGatewayCommand, CreatePaymentGatewayResponse>
{
    public async Task<CreatePaymentGatewayResponse> Handle(CreatePaymentGatewayCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var gateway = PaymentGateway.Create(
            request.Name,
            request.Provider,
            request.TransactionFeePercent,
            request.TransactionFeeFixed,
            request.MinTransactionAmount,
            request.MaxTransactionAmount);

        await repository.AddAsync(gateway, cancellationToken);
        logger.LogInformation("Payment gateway {Name} created with ID {Id}", gateway.Name, gateway.Id);

        return new CreatePaymentGatewayResponse(gateway.Id, gateway.Name);
    }
}

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Deactivate.v1;

/// <summary>
/// Command to deactivate a payment gateway.
/// </summary>
/// <param name="Id">The unique identifier of the payment gateway to deactivate.</param>
public sealed record DeactivatePaymentGatewayCommand(DefaultIdType Id) : IRequest<DeactivatePaymentGatewayResponse>;

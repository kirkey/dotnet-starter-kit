using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Activate.v1;

public sealed record ActivatePaymentGatewayCommand(DefaultIdType Id) : IRequest<ActivatePaymentGatewayResponse>;

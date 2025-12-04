using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Activate.v1;

public sealed record ActivatePaymentGatewayCommand(Guid Id) : IRequest<ActivatePaymentGatewayResponse>;

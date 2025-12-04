using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;

public sealed record GetPaymentGatewayRequest(Guid Id) : IRequest<PaymentGatewayResponse>;

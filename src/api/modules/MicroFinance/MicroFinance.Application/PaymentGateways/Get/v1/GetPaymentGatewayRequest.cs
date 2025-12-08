using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;

public sealed record GetPaymentGatewayRequest(DefaultIdType Id) : IRequest<PaymentGatewayResponse>;

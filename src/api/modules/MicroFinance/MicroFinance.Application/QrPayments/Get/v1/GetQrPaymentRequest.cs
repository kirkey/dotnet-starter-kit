using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;

public sealed record GetQrPaymentRequest(DefaultIdType Id) : IRequest<QrPaymentResponse>;

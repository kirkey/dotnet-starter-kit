using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;

public sealed record GetQrPaymentRequest(Guid Id) : IRequest<QrPaymentResponse>;

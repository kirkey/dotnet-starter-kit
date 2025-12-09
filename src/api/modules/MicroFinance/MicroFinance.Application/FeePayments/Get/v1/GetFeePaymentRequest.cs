using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Get.v1;

/// <summary>
/// Request to get a fee payment by ID.
/// </summary>
public sealed record GetFeePaymentRequest(DefaultIdType Id) : IRequest<FeePaymentResponse>;

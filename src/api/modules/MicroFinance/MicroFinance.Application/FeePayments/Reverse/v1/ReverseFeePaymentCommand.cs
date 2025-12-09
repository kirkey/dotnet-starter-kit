using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Reverse.v1;

/// <summary>
/// Command to reverse a fee payment.
/// </summary>
public sealed record ReverseFeePaymentCommand(
    DefaultIdType Id,
    string Reason) : IRequest<ReverseFeePaymentResponse>;

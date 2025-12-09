using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Create.v1;

/// <summary>
/// Command to create a new fee payment.
/// </summary>
public sealed record CreateFeePaymentCommand(
    DefaultIdType FeeChargeId,
    string Reference,
    decimal Amount,
    string PaymentMethod,
    string PaymentSource,
    DateOnly? PaymentDate = null,
    DefaultIdType? LoanRepaymentId = null,
    DefaultIdType? SavingsTransactionId = null,
    string? Notes = null) : IRequest<CreateFeePaymentResponse>;

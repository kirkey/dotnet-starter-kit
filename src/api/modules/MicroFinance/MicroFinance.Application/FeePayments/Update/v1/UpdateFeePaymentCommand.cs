using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Update.v1;

/// <summary>
/// Command to update a fee payment.
/// </summary>
public sealed record UpdateFeePaymentCommand(
    DefaultIdType Id,
    string? Reference = null,
    DateOnly? PaymentDate = null,
    decimal? Amount = null,
    string? PaymentMethod = null,
    string? PaymentSource = null,
    string? Notes = null) : IRequest<UpdateFeePaymentResponse>;

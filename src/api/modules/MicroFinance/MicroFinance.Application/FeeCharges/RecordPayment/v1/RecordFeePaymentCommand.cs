using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.RecordPayment.v1;

/// <summary>
/// Command to record a payment against a fee charge.
/// </summary>
public sealed record RecordFeePaymentCommand(Guid FeeChargeId, decimal Amount) : IRequest<RecordFeePaymentResponse>;

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.RecordPayment.v1;

/// <summary>
/// Command to record payment against a promise to pay.
/// </summary>
public sealed record RecordPromisePaymentCommand(
    DefaultIdType PromiseId,
    decimal Amount,
    DateOnly PaymentDate) : IRequest<RecordPromisePaymentResponse>;

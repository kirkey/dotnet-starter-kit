using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.RecordPayment.v1;

/// <summary>
/// Command to record payment against a promise to pay.
/// </summary>
public sealed record RecordPromisePaymentCommand(
    Guid PromiseId,
    decimal Amount,
    DateOnly PaymentDate) : IRequest<RecordPromisePaymentResponse>;

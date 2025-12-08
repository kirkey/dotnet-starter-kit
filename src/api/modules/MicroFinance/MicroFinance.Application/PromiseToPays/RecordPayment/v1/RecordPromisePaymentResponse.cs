namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.RecordPayment.v1;

/// <summary>
/// Response after recording payment against a promise.
/// </summary>
public sealed record RecordPromisePaymentResponse(
    DefaultIdType Id,
    decimal ActualAmountPaid,
    string Status);

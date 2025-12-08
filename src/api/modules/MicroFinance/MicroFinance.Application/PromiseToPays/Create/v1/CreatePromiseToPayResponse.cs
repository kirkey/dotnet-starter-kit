namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Create.v1;

/// <summary>
/// Response after creating a promise to pay.
/// </summary>
public sealed record CreatePromiseToPayResponse(
    DefaultIdType Id,
    DateOnly PromiseDate,
    DateOnly PromisedPaymentDate,
    decimal PromisedAmount,
    string Status);

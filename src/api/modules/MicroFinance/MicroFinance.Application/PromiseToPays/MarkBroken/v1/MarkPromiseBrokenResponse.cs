namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.MarkBroken.v1;

/// <summary>
/// Response after marking a promise as broken.
/// </summary>
public sealed record MarkPromiseBrokenResponse(DefaultIdType Id, string Status, string BreachReason);

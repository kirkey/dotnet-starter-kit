namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Resolve.v1;

/// <summary>
/// Response after resolving a customer case.
/// </summary>
public sealed record ResolveCustomerCaseResponse(DefaultIdType Id, string Status, DateTimeOffset ResolvedAt);

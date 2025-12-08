namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Close.v1;

/// <summary>
/// Response after closing a customer case.
/// </summary>
public sealed record CloseCustomerCaseResponse(DefaultIdType Id, string Status, DateTimeOffset ClosedAt);

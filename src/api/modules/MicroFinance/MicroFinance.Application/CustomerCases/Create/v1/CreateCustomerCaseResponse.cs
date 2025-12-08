namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Create.v1;

/// <summary>
/// Response after creating a customer case.
/// </summary>
public sealed record CreateCustomerCaseResponse(
    DefaultIdType Id,
    string CaseNumber,
    string Subject,
    string Status);

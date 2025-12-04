namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Escalate.v1;

/// <summary>
/// Response after escalating a customer case.
/// </summary>
public sealed record EscalateCustomerCaseResponse(Guid Id, int EscalationLevel, string Status);

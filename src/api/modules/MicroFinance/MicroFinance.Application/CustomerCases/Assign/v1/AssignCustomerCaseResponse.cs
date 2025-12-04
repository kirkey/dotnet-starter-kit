namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Assign.v1;

/// <summary>
/// Response after assigning a customer case.
/// </summary>
public sealed record AssignCustomerCaseResponse(Guid Id, Guid AssignedToId, string Status);

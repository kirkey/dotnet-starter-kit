using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Escalate.v1;

/// <summary>
/// Command to escalate a customer case.
/// </summary>
public sealed record EscalateCustomerCaseCommand(DefaultIdType CaseId, DefaultIdType EscalatedToId, string Reason) : IRequest<EscalateCustomerCaseResponse>;

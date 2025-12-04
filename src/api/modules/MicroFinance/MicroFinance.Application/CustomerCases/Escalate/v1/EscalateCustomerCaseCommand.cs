using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Escalate.v1;

/// <summary>
/// Command to escalate a customer case.
/// </summary>
public sealed record EscalateCustomerCaseCommand(Guid CaseId, Guid EscalatedToId, string Reason) : IRequest<EscalateCustomerCaseResponse>;

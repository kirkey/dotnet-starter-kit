using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Assign.v1;

/// <summary>
/// Command to assign a customer case to a staff member.
/// </summary>
public sealed record AssignCustomerCaseCommand(Guid CaseId, Guid StaffId) : IRequest<AssignCustomerCaseResponse>;

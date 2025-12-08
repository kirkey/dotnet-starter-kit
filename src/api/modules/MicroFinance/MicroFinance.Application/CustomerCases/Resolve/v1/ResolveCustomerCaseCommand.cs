using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Resolve.v1;

/// <summary>
/// Command to resolve a customer case.
/// </summary>
public sealed record ResolveCustomerCaseCommand(DefaultIdType CaseId, string Resolution) : IRequest<ResolveCustomerCaseResponse>;

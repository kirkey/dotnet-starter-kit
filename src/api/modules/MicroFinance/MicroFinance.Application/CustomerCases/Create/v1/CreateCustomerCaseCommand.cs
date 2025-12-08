using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Create.v1;

/// <summary>
/// Command to create a new customer case.
/// </summary>
public sealed record CreateCustomerCaseCommand(
    string CaseNumber,
    DefaultIdType MemberId,
    string Subject,
    string Category,
    string Description,
    string Channel,
    string Priority = "Medium",
    int SlaHours = 24) : IRequest<CreateCustomerCaseResponse>;

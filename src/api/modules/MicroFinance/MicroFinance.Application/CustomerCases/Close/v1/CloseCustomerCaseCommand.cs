using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Close.v1;

/// <summary>
/// Command to close a customer case.
/// </summary>
public sealed record CloseCustomerCaseCommand(
    DefaultIdType CaseId,
    int? SatisfactionScore = null,
    string? Feedback = null) : IRequest<CloseCustomerCaseResponse>;

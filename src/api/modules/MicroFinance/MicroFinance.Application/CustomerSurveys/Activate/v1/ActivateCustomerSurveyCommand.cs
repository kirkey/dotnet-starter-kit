using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Activate.v1;

/// <summary>
/// Command to activate a customer survey.
/// </summary>
public sealed record ActivateCustomerSurveyCommand(Guid Id) : IRequest<ActivateCustomerSurveyResponse>;

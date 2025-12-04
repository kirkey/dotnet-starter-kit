using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Complete.v1;

/// <summary>
/// Command to complete a customer survey.
/// </summary>
public sealed record CompleteCustomerSurveyCommand(Guid Id) : IRequest<CompleteCustomerSurveyResponse>;

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Complete.v1;

/// <summary>
/// Command to complete a customer survey.
/// </summary>
public sealed record CompleteCustomerSurveyCommand(DefaultIdType Id) : IRequest<CompleteCustomerSurveyResponse>;

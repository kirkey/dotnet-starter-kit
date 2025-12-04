using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Create.v1;

/// <summary>
/// Command to create a new customer survey.
/// </summary>
public sealed record CreateCustomerSurveyCommand(
    string Title,
    string SurveyType,
    DateOnly StartDate,
    string? Description = null,
    DateOnly? EndDate = null,
    bool IsAnonymous = true) : IRequest<CreateCustomerSurveyResponse>;

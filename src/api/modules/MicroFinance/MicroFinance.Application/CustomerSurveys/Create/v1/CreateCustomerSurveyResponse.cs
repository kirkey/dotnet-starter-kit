namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Create.v1;

/// <summary>
/// Response from creating a customer survey.
/// </summary>
public sealed record CreateCustomerSurveyResponse(DefaultIdType Id, string Title, string Status);

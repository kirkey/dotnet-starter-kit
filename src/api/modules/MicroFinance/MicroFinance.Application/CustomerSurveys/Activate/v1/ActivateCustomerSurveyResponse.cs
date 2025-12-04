namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Activate.v1;

/// <summary>
/// Response from activating a customer survey.
/// </summary>
public sealed record ActivateCustomerSurveyResponse(Guid Id, string Status);

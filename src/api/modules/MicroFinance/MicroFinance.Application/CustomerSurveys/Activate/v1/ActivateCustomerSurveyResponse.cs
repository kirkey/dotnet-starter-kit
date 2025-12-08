namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Activate.v1;

/// <summary>
/// Response from activating a customer survey.
/// </summary>
public sealed record ActivateCustomerSurveyResponse(DefaultIdType Id, string Status);

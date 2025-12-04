namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Complete.v1;

/// <summary>
/// Response from completing a customer survey.
/// </summary>
public sealed record CompleteCustomerSurveyResponse(Guid Id, string Status, int TotalResponses, decimal? AverageScore);

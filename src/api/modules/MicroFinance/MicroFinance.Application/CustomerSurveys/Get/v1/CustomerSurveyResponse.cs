namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Get.v1;

/// <summary>
/// Response containing customer survey details.
/// </summary>
public sealed record CustomerSurveyResponse(
    DefaultIdType Id,
    string Title,
    string? Description,
    string SurveyType,
    string Status,
    DateOnly StartDate,
    DateOnly? EndDate,
    int TotalResponses,
    decimal? AverageScore,
    int? NpsScore,
    bool IsAnonymous);

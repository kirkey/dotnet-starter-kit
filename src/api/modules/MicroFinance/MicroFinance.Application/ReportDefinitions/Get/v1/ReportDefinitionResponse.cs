namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;

public sealed record ReportDefinitionResponse(
    Guid Id,
    string Code,
    string Name,
    string Category,
    string OutputFormat,
    string? Description,
    string? ParametersDefinition,
    bool IsScheduled,
    string? ScheduleFrequency,
    int? ScheduleDay,
    TimeOnly? ScheduleTime,
    DateTime? LastGeneratedAt,
    string Status,
    DateTimeOffset CreatedOn);

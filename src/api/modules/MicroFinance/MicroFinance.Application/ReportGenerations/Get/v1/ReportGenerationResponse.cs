namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;

public sealed record ReportGenerationResponse(
    DefaultIdType Id,
    DefaultIdType ReportDefinitionId,
    DefaultIdType? RequestedByUserId,
    string Trigger,
    string? Parameters,
    DateOnly? ReportStartDate,
    DateOnly? ReportEndDate,
    DefaultIdType? BranchId,
    string? OutputFormat,
    string? OutputFile,
    long? FileSizeBytes,
    int? RecordCount,
    string Status,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    long? DurationMs,
    string? ErrorMessage,
    DateTimeOffset CreatedOn);

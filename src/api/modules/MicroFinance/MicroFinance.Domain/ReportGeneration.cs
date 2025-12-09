using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents an instance of a generated report.
/// Tracks report generation history and output files.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track execution history of automated scheduled reports.
/// - Store generated report files (PDF, Excel, CSV) with download links.
/// - Audit trail for regulatory report submissions.
/// - Monitor report generation performance and failures.
/// - Queue management for large batch reports.
/// - User-triggered ad-hoc report generation tracking.
/// 
/// Default values and constraints:
/// - Trigger: Manual, Scheduled, API
/// - Status: Queued by default (Queued, Processing, Completed, Failed)
/// - OutputFile: file path or URL, max 512 characters
/// - ErrorMessage: error details if failed, max 4096 characters
/// - GenerationTime: duration in seconds
/// 
/// Business rules:
/// - Large reports queued for background processing.
/// - Failed reports logged with error details for debugging.
/// - Output files retained per archival policy.
/// - Scheduled reports auto-retry on transient failures.
/// - User notified when report ready for download.
/// </remarks>
/// <seealso cref="ReportDefinition"/>
/// <seealso cref="Staff"/>
/// <seealso cref="Document"/>
public sealed class ReportGeneration : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants.
    /// </summary>
    public static class MaxLengths
    {
        public const int OutputFile = 512;
        public const int ErrorMessage = 4096;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Generation trigger.
    /// </summary>
    public const string TriggerManual = "Manual";
    public const string TriggerScheduled = "Scheduled";
    public const string TriggerApi = "API";

    /// <summary>
    /// Status values.
    /// </summary>
    public const string StatusQueued = "Queued";
    public const string StatusProcessing = "Processing";
    public const string StatusCompleted = "Completed";
    public const string StatusFailed = "Failed";
    public const string StatusCancelled = "Cancelled";

    /// <summary>
    /// Reference to the report definition.
    /// </summary>
    public Guid ReportDefinitionId { get; private set; }

    /// <summary>
    /// User who requested the report.
    /// </summary>
    public Guid? RequestedByUserId { get; private set; }

    /// <summary>
    /// How the report was triggered.
    /// </summary>
    public string Trigger { get; private set; } = TriggerManual;

    /// <summary>
    /// Parameters used for generation (JSON).
    /// </summary>
    public string? Parameters { get; private set; }

    /// <summary>
    /// Report start date (for date-ranged reports).
    /// </summary>
    public DateOnly? ReportStartDate { get; private set; }

    /// <summary>
    /// Report end date (for date-ranged reports).
    /// </summary>
    public DateOnly? ReportEndDate { get; private set; }

    /// <summary>
    /// Branch filter (if branch-specific).
    /// </summary>
    public Guid? BranchId { get; private set; }

    /// <summary>
    /// Output format used.
    /// </summary>
    public string? OutputFormat { get; private set; }

    /// <summary>
    /// Path to the output file.
    /// </summary>
    public string? OutputFile { get; private set; }

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long? FileSizeBytes { get; private set; }

    /// <summary>
    /// Number of records in the report.
    /// </summary>
    public int? RecordCount { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusQueued;

    /// <summary>
    /// Time when processing started.
    /// </summary>
    public DateTime? StartedAt { get; private set; }

    /// <summary>
    /// Time when processing completed.
    /// </summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>
    /// Processing duration in milliseconds.
    /// </summary>
    public long? DurationMs { get; private set; }

    /// <summary>
    /// Error message (if failed).
    /// </summary>
    public string? ErrorMessage { get; private set; }

    // Navigation properties
    public ReportDefinition ReportDefinition { get; private set; } = null!;

    private ReportGeneration() { }

    /// <summary>
    /// Queues a new report generation.
    /// </summary>
    public static ReportGeneration Queue(
        Guid reportDefinitionId,
        string trigger,
        string outputFormat,
        Guid? requestedByUserId = null,
        string? parameters = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        Guid? branchId = null)
    {
        var generation = new ReportGeneration
        {
            ReportDefinitionId = reportDefinitionId,
            RequestedByUserId = requestedByUserId,
            Trigger = trigger,
            OutputFormat = outputFormat,
            Parameters = parameters,
            ReportStartDate = startDate,
            ReportEndDate = endDate,
            BranchId = branchId,
            Status = StatusQueued
        };

        generation.QueueDomainEvent(new ReportGenerationQueued(generation));
        return generation;
    }

    /// <summary>
    /// Starts processing the report.
    /// </summary>
    public void StartProcessing()
    {
        if (Status != StatusQueued)
            throw new InvalidOperationException("Only queued reports can start processing.");

        StartedAt = DateTime.UtcNow;
        Status = StatusProcessing;
        QueueDomainEvent(new ReportGenerationStarted(Id));
    }

    /// <summary>
    /// Completes the report generation successfully.
    /// </summary>
    public void Complete(string outputFile, long fileSizeBytes, int recordCount)
    {
        if (Status != StatusProcessing)
            throw new InvalidOperationException("Only processing reports can be completed.");

        CompletedAt = DateTime.UtcNow;
        if (StartedAt.HasValue)
        {
            DurationMs = (long)(CompletedAt.Value - StartedAt.Value).TotalMilliseconds;
        }

        OutputFile = outputFile;
        FileSizeBytes = fileSizeBytes;
        RecordCount = recordCount;
        Status = StatusCompleted;

        QueueDomainEvent(new ReportGenerationCompleted(Id, outputFile, recordCount));
    }

    /// <summary>
    /// Marks the generation as failed.
    /// </summary>
    public void Fail(string errorMessage)
    {
        CompletedAt = DateTime.UtcNow;
        if (StartedAt.HasValue)
        {
            DurationMs = (long)(CompletedAt.Value - StartedAt.Value).TotalMilliseconds;
        }

        ErrorMessage = errorMessage;
        Status = StatusFailed;

        QueueDomainEvent(new ReportGenerationFailed(Id, errorMessage));
    }

    /// <summary>
    /// Cancels the report generation.
    /// </summary>
    public void Cancel(string? reason = null)
    {
        if (Status != StatusQueued && Status != StatusProcessing)
            throw new InvalidOperationException("Only queued or processing reports can be cancelled.");

        if (reason is not null) Notes = reason;
        Status = StatusCancelled;

        QueueDomainEvent(new ReportGenerationCancelled(Id));
    }
}

using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a report definition/template for generating MFI reports.
/// Defines report structure, parameters, and scheduling.
/// </summary>
public sealed class ReportDefinition : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants.
    /// </summary>
    public static class MaxLengths
    {
        public const int Code = 32;
        public const int Name = 128;
        public const int Description = 4096;
        public const int Category = 64;
        public const int OutputFormat = 32;
        public const int Query = 16384;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Report category classification.
    /// </summary>
    public const string CategoryFinancial = "Financial";
    public const string CategoryOperational = "Operational";
    public const string CategoryLoan = "Loan";
    public const string CategorySavings = "Savings";
    public const string CategoryMember = "Member";
    public const string CategoryCompliance = "Compliance";
    public const string CategoryRisk = "Risk";
    public const string CategoryPerformance = "Performance";
    public const string CategoryAudit = "Audit";
    public const string CategoryRegulatory = "Regulatory";

    /// <summary>
    /// Output format.
    /// </summary>
    public const string FormatPdf = "PDF";
    public const string FormatExcel = "Excel";
    public const string FormatCsv = "CSV";
    public const string FormatHtml = "HTML";
    public const string FormatJson = "JSON";

    /// <summary>
    /// Schedule frequency.
    /// </summary>
    public const string FrequencyDaily = "Daily";
    public const string FrequencyWeekly = "Weekly";
    public const string FrequencyMonthly = "Monthly";
    public const string FrequencyQuarterly = "Quarterly";
    public const string FrequencyAnnual = "Annual";
    public const string FrequencyOnDemand = "OnDemand";

    /// <summary>
    /// Status values.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusDraft = "Draft";

    /// <summary>
    /// Unique report code.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Report category.
    /// </summary>
    public string Category { get; private set; } = CategoryFinancial;

    /// <summary>
    /// Default output format.
    /// </summary>
    public string OutputFormat { get; private set; } = FormatPdf;

    /// <summary>
    /// Report parameters definition (JSON).
    /// </summary>
    public string? ParametersDefinition { get; private set; }

    /// <summary>
    /// Report query or template.
    /// </summary>
    public string? Query { get; private set; }

    /// <summary>
    /// Report layout template (JSON).
    /// </summary>
    public string? LayoutTemplate { get; private set; }

    /// <summary>
    /// Whether report is scheduled.
    /// </summary>
    public bool IsScheduled { get; private set; }

    /// <summary>
    /// Schedule frequency.
    /// </summary>
    public string? ScheduleFrequency { get; private set; }

    /// <summary>
    /// Day of week/month for scheduled run.
    /// </summary>
    public int? ScheduleDay { get; private set; }

    /// <summary>
    /// Time of day for scheduled run.
    /// </summary>
    public TimeOnly? ScheduleTime { get; private set; }

    /// <summary>
    /// Recipients for scheduled reports (JSON array).
    /// </summary>
    public string? ScheduleRecipients { get; private set; }

    /// <summary>
    /// Last generated date.
    /// </summary>
    public DateTime? LastGeneratedAt { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusDraft;

    // Navigation properties
    public ICollection<ReportGeneration> Generations { get; private set; } = new List<ReportGeneration>();

    private ReportDefinition() { }

    /// <summary>
    /// Creates a new report definition.
    /// </summary>
    public static ReportDefinition Create(
        string code,
        string name,
        string category,
        string outputFormat,
        string? description = null,
        string? parametersDefinition = null,
        string? query = null,
        string? layoutTemplate = null)
    {
        var report = new ReportDefinition
        {
            Code = code,
            Name = name,
            Category = category,
            OutputFormat = outputFormat,
            Description = description,
            ParametersDefinition = parametersDefinition,
            Query = query,
            LayoutTemplate = layoutTemplate,
            IsScheduled = false,
            Status = StatusDraft
        };

        report.QueueDomainEvent(new ReportDefinitionCreated(report));
        return report;
    }

    /// <summary>
    /// Updates the report definition.
    /// </summary>
    public ReportDefinition Update(
        string? name,
        string? description,
        string? parametersDefinition,
        string? query,
        string? layoutTemplate,
        string? outputFormat,
        string? notes)
    {
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        if (parametersDefinition is not null) ParametersDefinition = parametersDefinition;
        if (query is not null) Query = query;
        if (layoutTemplate is not null) LayoutTemplate = layoutTemplate;
        if (outputFormat is not null) OutputFormat = outputFormat;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new ReportDefinitionUpdated(this));
        return this;
    }

    /// <summary>
    /// Configures schedule for the report.
    /// </summary>
    public void ConfigureSchedule(
        string frequency,
        int? day,
        TimeOnly? time,
        string? recipients)
    {
        IsScheduled = true;
        ScheduleFrequency = frequency;
        ScheduleDay = day;
        ScheduleTime = time;
        ScheduleRecipients = recipients;

        QueueDomainEvent(new ReportScheduleConfigured(Id, frequency));
    }

    /// <summary>
    /// Disables the schedule.
    /// </summary>
    public void DisableSchedule()
    {
        IsScheduled = false;
        QueueDomainEvent(new ReportScheduleDisabled(Id));
    }

    /// <summary>
    /// Activates the report definition.
    /// </summary>
    public void Activate()
    {
        Status = StatusActive;
        QueueDomainEvent(new ReportDefinitionActivated(Id));
    }

    /// <summary>
    /// Deactivates the report definition.
    /// </summary>
    public void Deactivate()
    {
        Status = StatusInactive;
        QueueDomainEvent(new ReportDefinitionDeactivated(Id));
    }

    /// <summary>
    /// Records report generation.
    /// </summary>
    public void RecordGeneration()
    {
        LastGeneratedAt = DateTime.UtcNow;
    }
}

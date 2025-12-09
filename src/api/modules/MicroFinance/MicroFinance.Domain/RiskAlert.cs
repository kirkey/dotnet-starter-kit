using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a risk alert triggered when risk thresholds are breached.
/// Supports risk monitoring, escalation workflows, and resolution tracking.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Auto-generate alerts when KRI thresholds are breached</description></item>
///   <item><description>Escalate unresolved alerts to senior management</description></item>
///   <item><description>Track alert resolution and corrective actions</description></item>
///   <item><description>Provide dashboard for active risk monitoring</description></item>
///   <item><description>Support regulatory risk reporting</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Risk alerts enable proactive risk management:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Early Warning</strong>: Detect problems before they escalate</description></item>
///   <item><description><strong>Accountability</strong>: Assign ownership for resolution</description></item>
///   <item><description><strong>Escalation</strong>: Auto-escalate unresolved alerts</description></item>
///   <item><description><strong>Audit Trail</strong>: Document all risk events and responses</description></item>
/// </list>
/// <para><strong>Severity Levels:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Low</strong>: Informational, monitor situation</description></item>
///   <item><description><strong>Medium</strong>: Requires attention within defined SLA</description></item>
///   <item><description><strong>High</strong>: Urgent, requires prompt action</description></item>
///   <item><description><strong>Critical</strong>: Emergency, requires immediate response</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="RiskIndicator"/> - KRI that triggered the alert</description></item>
///   <item><description><see cref="RiskCategory"/> - Risk category for classification</description></item>
///   <item><description><see cref="Staff"/> - Staff assigned to resolve</description></item>
/// </list>
/// </remarks>
public sealed class RiskAlert : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int AlertNumber = 32;
        public const int Title = 256;
        public const int Description = 4096;
        public const int Source = 128;
        public const int Resolution = 4096;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Alert severity levels.
    /// </summary>
    public const string SeverityLow = "Low";
    public const string SeverityMedium = "Medium";
    public const string SeverityHigh = "High";
    public const string SeverityCritical = "Critical";

    /// <summary>
    /// Alert status values.
    /// </summary>
    public const string StatusNew = "New";
    public const string StatusAcknowledged = "Acknowledged";
    public const string StatusInvestigating = "Investigating";
    public const string StatusMitigating = "Mitigating";
    public const string StatusResolved = "Resolved";
    public const string StatusClosed = "Closed";
    public const string StatusFalsePositive = "FalsePositive";

    /// <summary>
    /// Alert trigger source.
    /// </summary>
    public const string SourceAutomatic = "Automatic";
    public const string SourceManual = "Manual";
    public const string SourceThreshold = "Threshold";
    public const string SourceAudit = "Audit";
    public const string SourceExternal = "External";

    /// <summary>
    /// Reference to the risk category.
    /// </summary>
    public Guid? RiskCategoryId { get; private set; }

    /// <summary>
    /// Reference to the risk indicator that triggered this alert.
    /// </summary>
    public Guid? RiskIndicatorId { get; private set; }

    /// <summary>
    /// Unique alert number for tracking.
    /// </summary>
    public string AlertNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Alert title.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Detailed description of the risk alert.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Severity level.
    /// </summary>
    public string Severity { get; private set; } = SeverityMedium;

    /// <summary>
    /// Source of the alert.
    /// </summary>
    public string Source { get; private set; } = SourceAutomatic;

    /// <summary>
    /// Threshold value that was breached.
    /// </summary>
    public decimal? ThresholdValue { get; private set; }

    /// <summary>
    /// Actual value that triggered the breach.
    /// </summary>
    public decimal? ActualValue { get; private set; }

    /// <summary>
    /// Variance from the threshold.
    /// </summary>
    public decimal? Variance { get; private set; }

    /// <summary>
    /// Date and time when the alert was triggered.
    /// </summary>
    public DateTime AlertedAt { get; private set; }

    /// <summary>
    /// Reference to the branch if branch-specific.
    /// </summary>
    public Guid? BranchId { get; private set; }

    /// <summary>
    /// Reference to a loan if loan-specific.
    /// </summary>
    public Guid? LoanId { get; private set; }

    /// <summary>
    /// Reference to a member if member-specific.
    /// </summary>
    public Guid? MemberId { get; private set; }

    /// <summary>
    /// Current status of the alert.
    /// </summary>
    public string Status { get; private set; } = StatusNew;

    /// <summary>
    /// User who acknowledged the alert.
    /// </summary>
    public Guid? AcknowledgedByUserId { get; private set; }

    /// <summary>
    /// Date when acknowledged.
    /// </summary>
    public DateTime? AcknowledgedAt { get; private set; }

    /// <summary>
    /// User assigned to investigate/resolve.
    /// </summary>
    public Guid? AssignedToUserId { get; private set; }

    /// <summary>
    /// Date when assigned.
    /// </summary>
    public DateTime? AssignedAt { get; private set; }

    /// <summary>
    /// Resolution description.
    /// </summary>
    public string? Resolution { get; private set; }

    /// <summary>
    /// User who resolved the alert.
    /// </summary>
    public Guid? ResolvedByUserId { get; private set; }

    /// <summary>
    /// Date when resolved.
    /// </summary>
    public DateTime? ResolvedAt { get; private set; }

    /// <summary>
    /// Whether this alert was escalated.
    /// </summary>
    public bool IsEscalated { get; private set; }

    /// <summary>
    /// Escalation level (1, 2, 3...).
    /// </summary>
    public int EscalationLevel { get; private set; }

    /// <summary>
    /// Date when escalated.
    /// </summary>
    public DateTime? EscalatedAt { get; private set; }

    /// <summary>
    /// Due date for resolution.
    /// </summary>
    public DateTime? DueDate { get; private set; }

    /// <summary>
    /// Whether this alert is overdue.
    /// </summary>
    public bool IsOverdue => DueDate.HasValue && DateTime.UtcNow > DueDate.Value && Status != StatusResolved && Status != StatusClosed;

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public RiskCategory? RiskCategory { get; private set; }
    public RiskIndicator? RiskIndicator { get; private set; }
    public Branch? Branch { get; private set; }
    public Loan? Loan { get; private set; }
    public Member? Member { get; private set; }

    private RiskAlert() { }

    /// <summary>
    /// Creates a new risk alert.
    /// </summary>
    public static RiskAlert Create(
        string alertNumber,
        string title,
        string severity,
        string source,
        Guid? riskCategoryId = null,
        Guid? riskIndicatorId = null,
        string? description = null,
        decimal? thresholdValue = null,
        decimal? actualValue = null,
        Guid? branchId = null,
        Guid? loanId = null,
        Guid? memberId = null,
        DateTime? dueDate = null)
    {
        var alert = new RiskAlert
        {
            AlertNumber = alertNumber,
            Title = title,
            Severity = severity,
            Source = source,
            RiskCategoryId = riskCategoryId,
            RiskIndicatorId = riskIndicatorId,
            Description = description,
            ThresholdValue = thresholdValue,
            ActualValue = actualValue,
            BranchId = branchId,
            LoanId = loanId,
            MemberId = memberId,
            AlertedAt = DateTime.UtcNow,
            DueDate = dueDate,
            Status = StatusNew,
            IsEscalated = false,
            EscalationLevel = 0
        };

        if (thresholdValue.HasValue && actualValue.HasValue)
        {
            alert.Variance = actualValue.Value - thresholdValue.Value;
        }

        alert.QueueDomainEvent(new RiskAlertCreated(alert));
        return alert;
    }

    /// <summary>
    /// Acknowledges the alert.
    /// </summary>
    public void Acknowledge(Guid userId)
    {
        if (Status != StatusNew)
            throw new InvalidOperationException("Only new alerts can be acknowledged.");

        AcknowledgedByUserId = userId;
        AcknowledgedAt = DateTime.UtcNow;
        Status = StatusAcknowledged;

        QueueDomainEvent(new RiskAlertAcknowledged(Id, userId));
    }

    /// <summary>
    /// Assigns the alert for investigation.
    /// </summary>
    public void Assign(Guid userId)
    {
        AssignedToUserId = userId;
        AssignedAt = DateTime.UtcNow;
        Status = StatusInvestigating;

        QueueDomainEvent(new RiskAlertAssigned(Id, userId));
    }

    /// <summary>
    /// Updates the status to mitigating.
    /// </summary>
    public void StartMitigation()
    {
        Status = StatusMitigating;
        QueueDomainEvent(new RiskAlertMitigationStarted(Id));
    }

    /// <summary>
    /// Escalates the alert to a higher level.
    /// </summary>
    public void Escalate()
    {
        IsEscalated = true;
        EscalationLevel++;
        EscalatedAt = DateTime.UtcNow;

        // Increase severity if not already critical
        if (Severity == SeverityLow) Severity = SeverityMedium;
        else if (Severity == SeverityMedium) Severity = SeverityHigh;
        else if (Severity == SeverityHigh) Severity = SeverityCritical;

        QueueDomainEvent(new RiskAlertEscalated(Id, EscalationLevel, Severity));
    }

    /// <summary>
    /// Resolves the alert.
    /// </summary>
    public void Resolve(Guid userId, string resolution)
    {
        ResolvedByUserId = userId;
        ResolvedAt = DateTime.UtcNow;
        Resolution = resolution;
        Status = StatusResolved;

        QueueDomainEvent(new RiskAlertResolved(Id, userId, resolution));
    }

    /// <summary>
    /// Closes the alert.
    /// </summary>
    public void Close(string? notes = null)
    {
        if (Status != StatusResolved)
            throw new InvalidOperationException("Only resolved alerts can be closed.");

        Status = StatusClosed;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new RiskAlertClosed(Id));
    }

    /// <summary>
    /// Marks the alert as a false positive.
    /// </summary>
    public void MarkAsFalsePositive(Guid userId, string reason)
    {
        ResolvedByUserId = userId;
        ResolvedAt = DateTime.UtcNow;
        Resolution = reason;
        Status = StatusFalsePositive;

        QueueDomainEvent(new RiskAlertMarkedFalsePositive(Id, userId, reason));
    }

    /// <summary>
    /// Updates notes on the alert.
    /// </summary>
    public void AddNotes(string notes)
    {
        Notes = string.IsNullOrEmpty(Notes) ? notes : $"{Notes}\n\n{notes}";
    }
}

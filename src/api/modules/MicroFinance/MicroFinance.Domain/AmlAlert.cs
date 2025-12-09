using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for AmlAlert entity.
/// </summary>
public static class AmlAlertConstants
{
    /// <summary>Maximum length for alert code. (2^6 = 64)</summary>
    public const int AlertCodeMaxLength = 64;

    /// <summary>Maximum length for alert type. (2^5 = 32)</summary>
    public const int AlertTypeMaxLength = 32;

    /// <summary>Maximum length for severity. (2^5 = 32)</summary>
    public const int SeverityMaxLength = 32;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for trigger rule. (2^8 = 256)</summary>
    public const int TriggerRuleMaxLength = 256;

    /// <summary>Maximum length for description. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>Maximum length for resolution notes. (2^12 = 4096)</summary>
    public const int ResolutionNotesMaxLength = 4096;
}

/// <summary>
/// Represents an Anti-Money Laundering (AML) alert.
/// </summary>
/// <remarks>
/// Use cases:
/// - Flag suspicious transaction patterns.
/// - Track unusual activity for regulatory reporting.
/// - Manage investigation workflow for AML cases.
/// - Support Suspicious Activity Report (SAR) filing.
/// - Monitor high-risk members and transactions.
/// 
/// Default values and constraints:
/// - AlertCode: Unique alert identifier (max 64 chars).
/// - AlertType: Type of suspicious activity (max 32 chars).
/// - Severity: LOW, MEDIUM, HIGH, CRITICAL (max 32 chars).
/// - Status: NEW, INVESTIGATING, RESOLVED, REPORTED (max 32 chars).
/// - TriggerRule: Rule that triggered the alert (max 256 chars).
/// - Description: Alert details (max 2048 chars).
/// - ResolutionNotes: Investigation and resolution notes (max 4096 chars).
/// 
/// Business rules:
/// - Alerts triggered when patterns match predefined rules.
/// - Each alert must be investigated and resolved.
/// - Documentation required for compliance.
/// - SAR filing for confirmed suspicious activity.
/// - High-risk members subject to enhanced monitoring.
/// </remarks>
public class AmlAlert : AuditableEntity, IAggregateRoot
{
    // Alert Types
    /// <summary>Large cash transaction.</summary>
    public const string TypeLargeCash = "LARGE_CASH";
    /// <summary>Structuring (multiple transactions below threshold).</summary>
    public const string TypeStructuring = "STRUCTURING";
    /// <summary>Unusual transaction pattern.</summary>
    public const string TypeUnusualPattern = "UNUSUAL_PATTERN";
    /// <summary>High-risk country involvement.</summary>
    public const string TypeHighRiskCountry = "HIGH_RISK_COUNTRY";
    /// <summary>Politically Exposed Person (PEP).</summary>
    public const string TypePep = "PEP";
    /// <summary>Rapid movement of funds.</summary>
    public const string TypeRapidMovement = "RAPID_MOVEMENT";
    /// <summary>Dormant account activity.</summary>
    public const string TypeDormantActivity = "DORMANT_ACTIVITY";
    /// <summary>Identity mismatch.</summary>
    public const string TypeIdentityMismatch = "IDENTITY_MISMATCH";
    /// <summary>Sanction list match.</summary>
    public const string TypeSanctionMatch = "SANCTION_MATCH";
    /// <summary>Third-party concern.</summary>
    public const string TypeThirdParty = "THIRD_PARTY";

    // Severities
    /// <summary>Low severity - routine review.</summary>
    public const string SeverityLow = "LOW";
    /// <summary>Medium severity - requires investigation.</summary>
    public const string SeverityMedium = "MEDIUM";
    /// <summary>High severity - urgent investigation.</summary>
    public const string SeverityHigh = "HIGH";
    /// <summary>Critical severity - immediate action required.</summary>
    public const string SeverityCritical = "CRITICAL";

    // Statuses
    /// <summary>Alert is new and unreviewed.</summary>
    public const string StatusNew = "NEW";
    /// <summary>Alert is under investigation.</summary>
    public const string StatusInvestigating = "INVESTIGATING";
    /// <summary>Alert escalated to compliance officer.</summary>
    public const string StatusEscalated = "ESCALATED";
    /// <summary>Alert cleared - no suspicious activity.</summary>
    public const string StatusCleared = "CLEARED";
    /// <summary>Alert confirmed - SAR to be filed.</summary>
    public const string StatusConfirmed = "CONFIRMED";
    /// <summary>SAR filed with regulator.</summary>
    public const string StatusSarFiled = "SAR_FILED";
    /// <summary>Alert closed.</summary>
    public const string StatusClosed = "CLOSED";

    /// <summary>Gets the unique alert code.</summary>
    public string AlertCode { get; private set; } = default!;

    /// <summary>Gets the member ID if member-related.</summary>
    public DefaultIdType? MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member? Member { get; private set; }

    /// <summary>Gets the transaction ID if triggered by a transaction.</summary>
    public DefaultIdType? TransactionId { get; private set; }

    /// <summary>Gets the account ID if account-related.</summary>
    public DefaultIdType? AccountId { get; private set; }

    /// <summary>Gets the alert type.</summary>
    public string AlertType { get; private set; } = default!;

    /// <summary>Gets the severity level.</summary>
    public string Severity { get; private set; } = default!;

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the rule that triggered the alert.</summary>
    public string TriggerRule { get; private set; } = default!;

    /// <summary>Gets the description of the suspicious activity.</summary>
    public string Description { get; private set; } = default!;

    /// <summary>Gets the transaction amount involved.</summary>
    public decimal? TransactionAmount { get; private set; }

    /// <summary>Gets the date the alert was generated.</summary>
    public DateTime AlertedAt { get; private set; }

    /// <summary>Gets the date investigation started.</summary>
    public DateTime? InvestigationStartedAt { get; private set; }

    /// <summary>Gets the date the alert was resolved.</summary>
    public DateTime? ResolvedAt { get; private set; }

    /// <summary>Gets the assigned investigator's user ID.</summary>
    public DefaultIdType? AssignedToId { get; private set; }

    /// <summary>Gets the user ID who resolved the alert.</summary>
    public DefaultIdType? ResolvedById { get; private set; }

    /// <summary>Gets the resolution notes.</summary>
    public string? ResolutionNotes { get; private set; }

    /// <summary>Gets the SAR reference number if filed.</summary>
    public string? SarReference { get; private set; }

    /// <summary>Gets the date SAR was filed.</summary>
    public DateOnly? SarFiledDate { get; private set; }

    /// <summary>Gets whether this requires regulatory reporting.</summary>
    public bool RequiresReporting { get; private set; }

    private AmlAlert() { }

    private AmlAlert(
        DefaultIdType id,
        string alertCode,
        string alertType,
        string severity,
        string triggerRule,
        string description)
    {
        Id = id;
        AlertCode = alertCode.Trim();
        AlertType = alertType;
        Severity = severity;
        Status = StatusNew;
        TriggerRule = triggerRule.Trim();
        Description = description.Trim();
        AlertedAt = DateTime.UtcNow;
        RequiresReporting = false;

        QueueDomainEvent(new AmlAlertCreated { AmlAlert = this });
    }

    /// <summary>Creates a new AmlAlert.</summary>
    public static AmlAlert Create(
        string alertCode,
        string alertType,
        string severity,
        string triggerRule,
        string description)
    {
        return new AmlAlert(
            DefaultIdType.NewGuid(),
            alertCode,
            alertType,
            severity,
            triggerRule,
            description);
    }

    /// <summary>Associates with a member.</summary>
    public AmlAlert ForMember(DefaultIdType memberId)
    {
        MemberId = memberId;
        return this;
    }

    /// <summary>Associates with a transaction.</summary>
    public AmlAlert ForTransaction(DefaultIdType transactionId, decimal amount)
    {
        TransactionId = transactionId;
        TransactionAmount = amount;
        return this;
    }

    /// <summary>Associates with an account.</summary>
    public AmlAlert ForAccount(DefaultIdType accountId)
    {
        AccountId = accountId;
        return this;
    }

    /// <summary>Assigns the alert for investigation.</summary>
    public AmlAlert AssignTo(DefaultIdType userId)
    {
        AssignedToId = userId;
        Status = StatusInvestigating;
        InvestigationStartedAt = DateTime.UtcNow;
        return this;
    }

    /// <summary>Escalates the alert.</summary>
    public AmlAlert Escalate(string reason)
    {
        Status = StatusEscalated;
        Description += $"\n[ESCALATED] {reason}";
        QueueDomainEvent(new AmlAlertEscalated { AlertId = Id, Reason = reason });
        return this;
    }

    /// <summary>Clears the alert as non-suspicious.</summary>
    public AmlAlert Clear(DefaultIdType resolvedById, string notes)
    {
        Status = StatusCleared;
        ResolvedById = resolvedById;
        ResolvedAt = DateTime.UtcNow;
        ResolutionNotes = notes?.Trim();
        return this;
    }

    /// <summary>Confirms suspicious activity.</summary>
    public AmlAlert ConfirmSuspicious(DefaultIdType resolvedById, string notes)
    {
        Status = StatusConfirmed;
        ResolvedById = resolvedById;
        ResolvedAt = DateTime.UtcNow;
        ResolutionNotes = notes?.Trim();
        RequiresReporting = true;
        
        QueueDomainEvent(new AmlAlertConfirmed { AlertId = Id, MemberId = MemberId });
        return this;
    }

    /// <summary>Records SAR filing.</summary>
    public AmlAlert FileSar(string sarReference, DateOnly filedDate)
    {
        if (Status != StatusConfirmed)
            throw new InvalidOperationException($"Cannot file SAR when status is {Status}.");

        SarReference = sarReference?.Trim();
        SarFiledDate = filedDate;
        Status = StatusSarFiled;
        
        QueueDomainEvent(new SarFiled { AlertId = Id, SarReference = sarReference });
        return this;
    }

    /// <summary>Closes the alert.</summary>
    public AmlAlert Close()
    {
        Status = StatusClosed;
        if (!ResolvedAt.HasValue)
        {
            ResolvedAt = DateTime.UtcNow;
        }
        return this;
    }
}

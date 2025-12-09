using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a risk category for classifying different types of organizational risks.
/// Provides hierarchical structure for risk management framework.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Organize risks into categories for reporting and analysis</description></item>
///   <item><description>Define risk appetite and tolerance per category</description></item>
///   <item><description>Assign risk owners to each category</description></item>
///   <item><description>Support regulatory risk classification requirements</description></item>
///   <item><description>Enable drill-down risk reporting by category</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Risk categories form the foundation of the MFI's risk management framework:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Basel Framework</strong>: Aligns with regulatory risk categories</description></item>
///   <item><description><strong>Risk Register</strong>: All identified risks mapped to categories</description></item>
///   <item><description><strong>Board Reporting</strong>: Risk dashboards organized by category</description></item>
/// </list>
/// <para><strong>Risk Types:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Credit</strong>: Borrower default risk</description></item>
///   <item><description><strong>Operational</strong>: Process, people, system failures</description></item>
///   <item><description><strong>Market</strong>: Interest rate, currency risk</description></item>
///   <item><description><strong>Liquidity</strong>: Cash flow and funding risk</description></item>
///   <item><description><strong>Compliance</strong>: Regulatory and legal risk</description></item>
///   <item><description><strong>Reputational</strong>: Brand and public perception</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="RiskIndicator"/> - KRIs within this category</description></item>
///   <item><description><see cref="RiskAlert"/> - Alerts classified to this category</description></item>
/// </list>
/// </remarks>
public sealed class RiskCategory : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int Code = 32;
        public const int Name = 128;
        public const int Description = 4096;
        public const int RiskType = 64;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Risk type classification.
    /// </summary>
    public const string TypeCredit = "Credit";
    public const string TypeOperational = "Operational";
    public const string TypeMarket = "Market";
    public const string TypeLiquidity = "Liquidity";
    public const string TypeCompliance = "Compliance";
    public const string TypeReputational = "Reputational";
    public const string TypeStrategic = "Strategic";
    public const string TypeCounterparty = "Counterparty";

    /// <summary>
    /// Severity levels.
    /// </summary>
    public const string SeverityLow = "Low";
    public const string SeverityMedium = "Medium";
    public const string SeverityHigh = "High";
    public const string SeverityCritical = "Critical";

    /// <summary>
    /// Status of the risk category.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusDeprecated = "Deprecated";

    /// <summary>
    /// Unique code for the risk category.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Display name of the risk category.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Detailed description of the risk category.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Type of risk (Credit, Operational, Market, etc.).
    /// </summary>
    public string RiskType { get; private set; } = TypeCredit;

    /// <summary>
    /// Parent risk category for hierarchical classification.
    /// </summary>
    public Guid? ParentCategoryId { get; private set; }

    /// <summary>
    /// Default severity level for risks in this category.
    /// </summary>
    public string DefaultSeverity { get; private set; } = SeverityMedium;

    /// <summary>
    /// Weight factor for risk scoring.
    /// </summary>
    public decimal WeightFactor { get; private set; } = 1.0m;

    /// <summary>
    /// Threshold value that triggers alerts.
    /// </summary>
    public decimal? AlertThreshold { get; private set; }

    /// <summary>
    /// Whether this category requires immediate escalation.
    /// </summary>
    public bool RequiresEscalation { get; private set; }

    /// <summary>
    /// Escalation timeframe in hours.
    /// </summary>
    public int? EscalationHours { get; private set; }

    /// <summary>
    /// Display order for sorting.
    /// </summary>
    public int DisplayOrder { get; private set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public RiskCategory? ParentCategory { get; private set; }
    public ICollection<RiskCategory> SubCategories { get; private set; } = new List<RiskCategory>();
    public ICollection<RiskIndicator> Indicators { get; private set; } = new List<RiskIndicator>();
    public ICollection<RiskAlert> Alerts { get; private set; } = new List<RiskAlert>();

    private RiskCategory() { }

    /// <summary>
    /// Creates a new risk category.
    /// </summary>
    public static RiskCategory Create(
        string code,
        string name,
        string riskType,
        string? description = null,
        Guid? parentCategoryId = null,
        string defaultSeverity = SeverityMedium,
        decimal weightFactor = 1.0m,
        decimal? alertThreshold = null,
        bool requiresEscalation = false,
        int? escalationHours = null,
        int displayOrder = 0)
    {
        var category = new RiskCategory
        {
            Code = code,
            Name = name,
            RiskType = riskType,
            Description = description,
            ParentCategoryId = parentCategoryId,
            DefaultSeverity = defaultSeverity,
            WeightFactor = weightFactor,
            AlertThreshold = alertThreshold,
            RequiresEscalation = requiresEscalation,
            EscalationHours = escalationHours,
            DisplayOrder = displayOrder,
            Status = StatusActive
        };

        category.QueueDomainEvent(new RiskCategoryCreated(category));
        return category;
    }

    /// <summary>
    /// Updates the risk category.
    /// </summary>
    public RiskCategory Update(
        string? name,
        string? description,
        string? defaultSeverity,
        decimal? weightFactor,
        decimal? alertThreshold,
        bool? requiresEscalation,
        int? escalationHours,
        int? displayOrder,
        string? notes)
    {
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        if (defaultSeverity is not null) DefaultSeverity = defaultSeverity;
        if (weightFactor.HasValue) WeightFactor = weightFactor.Value;
        if (alertThreshold.HasValue) AlertThreshold = alertThreshold.Value;
        if (requiresEscalation.HasValue) RequiresEscalation = requiresEscalation.Value;
        if (escalationHours.HasValue) EscalationHours = escalationHours.Value;
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new RiskCategoryUpdated(this));
        return this;
    }

    /// <summary>
    /// Activates the risk category.
    /// </summary>
    public void Activate()
    {
        if (Status == StatusActive) return;
        Status = StatusActive;
        QueueDomainEvent(new RiskCategoryActivated(Id));
    }

    /// <summary>
    /// Deactivates the risk category.
    /// </summary>
    public void Deactivate()
    {
        if (Status == StatusInactive) return;
        Status = StatusInactive;
        QueueDomainEvent(new RiskCategoryDeactivated(Id));
    }

    /// <summary>
    /// Deprecates the risk category.
    /// </summary>
    public void Deprecate()
    {
        Status = StatusDeprecated;
        QueueDomainEvent(new RiskCategoryDeprecated(Id));
    }
}

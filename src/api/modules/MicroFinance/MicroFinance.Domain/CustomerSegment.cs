using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a customer segment for member classification, marketing, and risk management.
/// Enables targeted product offers and differentiated service levels.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Classify members into marketing segments for targeted campaigns</description></item>
///   <item><description>Define risk-based segments for credit decisioning</description></item>
///   <item><description>Identify high-value members for relationship management</description></item>
///   <item><description>Track segment membership for analytics and reporting</description></item>
///   <item><description>Enable segment-based product eligibility rules</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Member segmentation drives personalized member experience:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Marketing</strong>: Target relevant products to appropriate segments</description></item>
///   <item><description><strong>Risk</strong>: Apply segment-specific credit policies</description></item>
///   <item><description><strong>Service</strong>: Differentiated service levels (e.g., VIP, standard)</description></item>
///   <item><description><strong>Pricing</strong>: Segment-based pricing strategies</description></item>
/// </list>
/// <para><strong>Segment Types:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Demographic</strong>: Age, gender, location, occupation</description></item>
///   <item><description><strong>Behavioral</strong>: Transaction patterns, product usage</description></item>
///   <item><description><strong>Risk</strong>: Credit risk classification</description></item>
///   <item><description><strong>Value</strong>: Profitability, lifetime value</description></item>
///   <item><description><strong>Lifecycle</strong>: New, active, dormant, churned</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Member"/> - Members classified into segments</description></item>
///   <item><description><see cref="MarketingCampaign"/> - Campaigns targeting segments</description></item>
/// </list>
/// </remarks>
public sealed class CustomerSegment : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int NameMaxLength = 128;
    public const int CodeMaxLength = 32;
    public const int DescriptionMaxLength = 512;
    public const int StatusMaxLength = 32;
    public const int CriteriaMaxLength = 2048;
    
    // Segment Status
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    
    // Segment Types
    public const string TypeDemographic = "Demographic";
    public const string TypeBehavioral = "Behavioral";
    public const string TypeRisk = "Risk";
    public const string TypeValue = "Value";
    public const string TypeLifecycle = "Lifecycle";

    public string Code { get; private set; } = default!;
    public string Status { get; private set; } = StatusActive;
    public string SegmentType { get; private set; } = default!;
    public string? SegmentCriteria { get; private set; }
    public int Priority { get; private set; }
    public int MemberCount { get; private set; }
    public decimal? MinIncomeLevel { get; private set; }
    public decimal? MaxIncomeLevel { get; private set; }
    public string? RiskLevel { get; private set; }
    public decimal? DefaultInterestModifier { get; private set; }
    public decimal? DefaultFeeModifier { get; private set; }
    public string? TargetProducts { get; private set; }
    public string? MarketingCampaigns { get; private set; }
    public DateOnly? LastUpdatedDate { get; private set; }
    public string? Color { get; private set; }
    public int DisplayOrder { get; private set; }

    private CustomerSegment() { }

    public static CustomerSegment Create(
        string name,
        string code,
        string segmentType,
        string? description = null,
        int priority = 0)
    {
        var segment = new CustomerSegment
        {
            Name = name,
            Code = code,
            SegmentType = segmentType,
            Description = description,
            Priority = priority,
            Status = StatusActive,
            MemberCount = 0
        };

        segment.QueueDomainEvent(new CustomerSegmentCreated(segment));
        return segment;
    }

    public CustomerSegment UpdateMemberCount(int count)
    {
        MemberCount = count;
        LastUpdatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        return this;
    }

    public CustomerSegment Activate()
    {
        Status = StatusActive;
        return this;
    }

    public CustomerSegment Deactivate()
    {
        Status = StatusInactive;
        return this;
    }

    public CustomerSegment Update(
        string? name = null,
        string? description = null,
        string? segmentCriteria = null,
        int? priority = null,
        decimal? minIncomeLevel = null,
        decimal? maxIncomeLevel = null,
        string? riskLevel = null,
        decimal? defaultInterestModifier = null,
        decimal? defaultFeeModifier = null,
        string? targetProducts = null,
        string? marketingCampaigns = null,
        string? color = null,
        int? displayOrder = null)
    {
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        if (segmentCriteria is not null) SegmentCriteria = segmentCriteria;
        if (priority.HasValue) Priority = priority.Value;
        if (minIncomeLevel.HasValue) MinIncomeLevel = minIncomeLevel.Value;
        if (maxIncomeLevel.HasValue) MaxIncomeLevel = maxIncomeLevel.Value;
        if (riskLevel is not null) RiskLevel = riskLevel;
        if (defaultInterestModifier.HasValue) DefaultInterestModifier = defaultInterestModifier.Value;
        if (defaultFeeModifier.HasValue) DefaultFeeModifier = defaultFeeModifier.Value;
        if (targetProducts is not null) TargetProducts = targetProducts;
        if (marketingCampaigns is not null) MarketingCampaigns = marketingCampaigns;
        if (color is not null) Color = color;
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;

        QueueDomainEvent(new CustomerSegmentUpdated(this));
        return this;
    }
}

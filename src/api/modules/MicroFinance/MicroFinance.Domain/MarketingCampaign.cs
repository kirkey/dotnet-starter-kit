using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a marketing campaign for customer outreach.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
/// <item>Product promotion campaigns for new loan or savings products</item>
/// <item>Customer retention programs targeting at-risk members</item>
/// <item>Cross-selling initiatives (e.g., offering insurance to loan customers)</item>
/// <item>Dormant account reactivation through targeted messaging</item>
/// <item>Financial literacy education campaigns</item>
/// <item>Seasonal promotions during harvest or festival periods</item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Marketing campaigns are essential for MFI growth and customer engagement. Campaigns
/// can target specific customer segments via SMS, email, mobile app notifications, or
/// branch staff outreach. Campaign analytics track response rates, conversion, and ROI
/// to optimize future marketing investments. Regulatory compliance requires opt-in/opt-out
/// management and message content review.
/// </para>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
/// <item><see cref="CustomerSegment"/> - Target audience definition</item>
/// <item><see cref="CommunicationTemplate"/> - Message templates for campaigns</item>
/// <item><see cref="CommunicationLog"/> - Individual message delivery tracking</item>
/// <item><see cref="Member"/> - Campaign recipients</item>
/// </list>
/// </remarks>
public sealed class MarketingCampaign : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int NameMaxLength = 128;
    public const int CodeMaxLength = 32;
    public const int DescriptionMaxLength = 1024;
    public const int StatusMaxLength = 32;
    public const int TypeMaxLength = 64;
    public const int ChannelsMaxLength = 256;
    public const int MessageMaxLength = 2048;
    
    // Campaign Status
    public const string StatusDraft = "Draft";
    public const string StatusScheduled = "Scheduled";
    public const string StatusActive = "Active";
    public const string StatusPaused = "Paused";
    public const string StatusCompleted = "Completed";
    public const string StatusCancelled = "Cancelled";
    
    // Campaign Types
    public const string TypePromotion = "Promotion";
    public const string TypeEducation = "Education";
    public const string TypeRetention = "Retention";
    public const string TypeAcquisition = "Acquisition";
    public const string TypeCrossSell = "CrossSell";
    public const string TypeReactivation = "Reactivation";

    public string Code { get; private set; } = default!;
    public string CampaignType { get; private set; } = default!;
    public string Status { get; private set; } = StatusDraft;
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public string? TargetSegments { get; private set; }
    public string? TargetProducts { get; private set; }
    public string Channels { get; private set; } = default!;
    public string? MessageTemplate { get; private set; }
    public decimal Budget { get; private set; }
    public decimal SpentAmount { get; private set; }
    public int TargetAudience { get; private set; }
    public int ReachedCount { get; private set; }
    public int ResponseCount { get; private set; }
    public int ConversionCount { get; private set; }
    public decimal? ResponseRate { get; private set; }
    public decimal? ConversionRate { get; private set; }
    public decimal? Roi { get; private set; }
    public Guid? CreatedById { get; private set; }
    public Guid? ApprovedById { get; private set; }
    public DateOnly? ApprovedDate { get; private set; }

    private MarketingCampaign() { }

    public static MarketingCampaign Create(
        string name,
        string code,
        string campaignType,
        DateOnly startDate,
        string channels,
        decimal budget,
        int targetAudience,
        string? description = null,
        DateOnly? endDate = null)
    {
        var campaign = new MarketingCampaign
        {
            Name = name,
            Code = code,
            CampaignType = campaignType,
            StartDate = startDate,
            EndDate = endDate,
            Channels = channels,
            Budget = budget,
            TargetAudience = targetAudience,
            Description = description,
            Status = StatusDraft,
            SpentAmount = 0,
            ReachedCount = 0,
            ResponseCount = 0,
            ConversionCount = 0
        };

        campaign.QueueDomainEvent(new MarketingCampaignCreated(campaign));
        return campaign;
    }

    public MarketingCampaign Approve(Guid approvedById)
    {
        ApprovedById = approvedById;
        ApprovedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        Status = StatusScheduled;
        QueueDomainEvent(new MarketingCampaignApproved(Id, Name));
        return this;
    }

    public MarketingCampaign Launch()
    {
        Status = StatusActive;
        QueueDomainEvent(new MarketingCampaignLaunched(Id, Name));
        return this;
    }

    public MarketingCampaign Pause()
    {
        Status = StatusPaused;
        return this;
    }

    public MarketingCampaign Complete()
    {
        Status = StatusCompleted;
        CalculateMetrics();
        QueueDomainEvent(new MarketingCampaignCompleted(Id, ConversionCount, Roi));
        return this;
    }

    public MarketingCampaign Cancel()
    {
        Status = StatusCancelled;
        return this;
    }

    public MarketingCampaign RecordReach(int count)
    {
        ReachedCount += count;
        return this;
    }

    public MarketingCampaign RecordResponse()
    {
        ResponseCount++;
        CalculateMetrics();
        return this;
    }

    public MarketingCampaign RecordConversion()
    {
        ConversionCount++;
        CalculateMetrics();
        return this;
    }

    public MarketingCampaign RecordSpending(decimal amount)
    {
        SpentAmount += amount;
        CalculateMetrics();
        return this;
    }

    private void CalculateMetrics()
    {
        if (ReachedCount > 0)
        {
            ResponseRate = (decimal)ResponseCount / ReachedCount * 100;
            ConversionRate = (decimal)ConversionCount / ReachedCount * 100;
        }
        // ROI calculation would need conversion revenue data
    }

    public MarketingCampaign Update(
        string? name = null,
        string? description = null,
        string? targetSegments = null,
        string? targetProducts = null,
        string? messageTemplate = null,
        decimal? budget = null,
        DateOnly? endDate = null)
    {
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        if (targetSegments is not null) TargetSegments = targetSegments;
        if (targetProducts is not null) TargetProducts = targetProducts;
        if (messageTemplate is not null) MessageTemplate = messageTemplate;
        if (budget.HasValue) Budget = budget.Value;
        if (endDate.HasValue) EndDate = endDate;

        QueueDomainEvent(new MarketingCampaignUpdated(this));
        return this;
    }
}

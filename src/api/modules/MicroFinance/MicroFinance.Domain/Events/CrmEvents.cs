using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Customer Relationship Management entities.
/// </summary>
/// 
// CustomerSegment Events
public sealed record CustomerSegmentCreated(CustomerSegment Segment) : DomainEvent;
public sealed record CustomerSegmentUpdated(CustomerSegment Segment) : DomainEvent;

// CustomerCase Events
public sealed record CustomerCaseCreated(CustomerCase Case) : DomainEvent;
public sealed record CustomerCaseUpdated(CustomerCase Case) : DomainEvent;
public sealed record CustomerCaseAssigned(Guid CaseId, Guid StaffId) : DomainEvent;
public sealed record CustomerCaseEscalated(Guid CaseId, int EscalationLevel, string Reason) : DomainEvent;
public sealed record CustomerCaseResolved(Guid CaseId, string Resolution) : DomainEvent;
public sealed record CustomerCaseClosed(Guid CaseId, int? SatisfactionScore) : DomainEvent;
public sealed record CustomerCaseSlaBreached(Guid CaseId, string CaseNumber) : DomainEvent;

// CustomerSurvey Events
public sealed record CustomerSurveyCreated(CustomerSurvey Survey) : DomainEvent;
public sealed record CustomerSurveyUpdated(CustomerSurvey Survey) : DomainEvent;
public sealed record CustomerSurveyActivated(Guid SurveyId, string Title) : DomainEvent;
public sealed record CustomerSurveyCompleted(Guid SurveyId, int TotalResponses, decimal? AverageScore) : DomainEvent;

// MarketingCampaign Events
public sealed record MarketingCampaignCreated(MarketingCampaign Campaign) : DomainEvent;
public sealed record MarketingCampaignUpdated(MarketingCampaign Campaign) : DomainEvent;
public sealed record MarketingCampaignApproved(Guid CampaignId, string Name) : DomainEvent;
public sealed record MarketingCampaignLaunched(Guid CampaignId, string Name) : DomainEvent;
public sealed record MarketingCampaignCompleted(Guid CampaignId, int Conversions, decimal? Roi) : DomainEvent;

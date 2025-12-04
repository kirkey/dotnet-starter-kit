using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>
/// Domain events for Risk Management entities: RiskCategory, RiskIndicator, RiskAlert, 
/// CreditBureauInquiry, CreditBureauReport, and CreditScore.
/// </summary>
/// 
// RiskCategory Events
public sealed record RiskCategoryCreated(RiskCategory Category) : DomainEvent;
public sealed record RiskCategoryUpdated(RiskCategory Category) : DomainEvent;
public sealed record RiskCategoryActivated(Guid CategoryId) : DomainEvent;
public sealed record RiskCategoryDeactivated(Guid CategoryId) : DomainEvent;
public sealed record RiskCategoryDeprecated(Guid CategoryId) : DomainEvent;

// RiskIndicator Events
public sealed record RiskIndicatorCreated(RiskIndicator Indicator) : DomainEvent;
public sealed record RiskIndicatorUpdated(RiskIndicator Indicator) : DomainEvent;
public sealed record RiskIndicatorMeasured(Guid IndicatorId, decimal Value, string Health) : DomainEvent;
public sealed record RiskIndicatorHealthChanged(
    Guid IndicatorId,
    string PreviousHealth,
    string NewHealth) : DomainEvent;

// RiskAlert Events
public sealed record RiskAlertCreated(RiskAlert Alert) : DomainEvent;
public sealed record RiskAlertAcknowledged(Guid AlertId, Guid UserId) : DomainEvent;
public sealed record RiskAlertAssigned(Guid AlertId, Guid AssignedToUserId) : DomainEvent;
public sealed record RiskAlertMitigationStarted(Guid AlertId) : DomainEvent;
public sealed record RiskAlertEscalated(Guid AlertId, int EscalationLevel, string Severity) : DomainEvent;
public sealed record RiskAlertResolved(Guid AlertId, Guid ResolvedByUserId, string Resolution) : DomainEvent;
public sealed record RiskAlertClosed(Guid AlertId) : DomainEvent;
public sealed record RiskAlertMarkedFalsePositive(Guid AlertId, Guid UserId, string Reason) : DomainEvent;

// CreditBureauInquiry Events
public sealed record CreditBureauInquiryCreated(CreditBureauInquiry Inquiry) : DomainEvent;
public sealed record CreditBureauInquiryCompleted(
    Guid InquiryId,
    string ReferenceNumber,
    int? CreditScore) : DomainEvent;
public sealed record CreditBureauInquiryFailed(Guid InquiryId, string ErrorMessage) : DomainEvent;
public sealed record CreditBureauInquiryNoMatch(Guid InquiryId) : DomainEvent;

// CreditBureauReport Events
public sealed record CreditBureauReportCreated(CreditBureauReport Report) : DomainEvent;
public sealed record CreditBureauReportUpdated(CreditBureauReport Report) : DomainEvent;
public sealed record CreditBureauReportExpired(Guid ReportId) : DomainEvent;
public sealed record CreditBureauReportDisputed(Guid ReportId, string Reason) : DomainEvent;

// CreditScore Events
public sealed record CreditScoreCreated(CreditScore Score) : DomainEvent;
public sealed record CreditScoreUpdated(CreditScore Score) : DomainEvent;
public sealed record CreditScoreSuperseded(Guid ScoreId) : DomainEvent;
public sealed record CreditScoreExpired(Guid ScoreId) : DomainEvent;

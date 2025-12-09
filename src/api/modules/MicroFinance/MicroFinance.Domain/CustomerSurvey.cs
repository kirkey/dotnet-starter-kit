using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a customer satisfaction survey.
/// </summary>
/// <remarks>
/// Use cases:
/// - Measure NPS (Net Promoter Score) for customer loyalty tracking.
/// - Collect product feedback for loan and savings products.
/// - Assess service quality after branch visits or transactions.
/// - Conduct post-loan disbursement satisfaction surveys.
/// - Perform annual customer experience research and benchmarking.
/// 
/// Default values and constraints:
/// - Title: required, max 256 characters (example: "Q1 2025 Customer Satisfaction Survey")
/// - Type: Satisfaction, NetPromoterScore, ProductFeedback, ServiceQuality, BranchFeedback
/// - Status: Draft by default (Draft, Active, Paused, Completed, Archived)
/// - Questions: JSON array of survey questions, max 4096 characters
/// 
/// Business rules:
/// - Surveys can target specific member segments.
/// - NPS score ranges from -100 to +100.
/// - Survey results analyzed at branch and overall levels.
/// - Low scores trigger follow-up actions.
/// - Anonymous surveys allowed for sensitive topics.
/// </remarks>
/// <seealso cref="Member"/>
/// <seealso cref="CustomerSegment"/>
/// <seealso cref="MarketingCampaign"/>
/// <seealso cref="Branch"/>
public sealed class CustomerSurvey : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int TitleMaxLength = 256;
    public const int DescriptionMaxLength = 1024;
    public const int StatusMaxLength = 32;
    public const int TypeMaxLength = 64;
    public const int QuestionsMaxLength = 4096;
    
    // Survey Status
    public const string StatusDraft = "Draft";
    public const string StatusActive = "Active";
    public const string StatusPaused = "Paused";
    public const string StatusCompleted = "Completed";
    public const string StatusArchived = "Archived";
    
    // Survey Types
    public const string TypeSatisfaction = "Satisfaction";
    public const string TypeNPS = "NetPromoterScore";
    public const string TypeProductFeedback = "ProductFeedback";
    public const string TypeServiceQuality = "ServiceQuality";
    public const string TypeBranchFeedback = "BranchFeedback";

    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public string SurveyType { get; private set; } = default!;
    public string Status { get; private set; } = StatusDraft;
    public string? Questions { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public string? TargetSegments { get; private set; }
    public int TotalResponses { get; private set; }
    public decimal? AverageScore { get; private set; }
    public int? NpsScore { get; private set; }
    public Guid? BranchId { get; private set; }
    public bool IsAnonymous { get; private set; }
    public bool SendReminders { get; private set; }
    public int ReminderDays { get; private set; }
    public string? ThankYouMessage { get; private set; }

    private CustomerSurvey() { }

    public static CustomerSurvey Create(
        string title,
        string surveyType,
        DateOnly startDate,
        string? description = null,
        DateOnly? endDate = null,
        bool isAnonymous = true)
    {
        var survey = new CustomerSurvey
        {
            Title = title,
            SurveyType = surveyType,
            StartDate = startDate,
            EndDate = endDate,
            Description = description,
            IsAnonymous = isAnonymous,
            Status = StatusDraft,
            TotalResponses = 0
        };

        survey.QueueDomainEvent(new CustomerSurveyCreated(survey));
        return survey;
    }

    public CustomerSurvey Activate()
    {
        Status = StatusActive;
        QueueDomainEvent(new CustomerSurveyActivated(Id, Title));
        return this;
    }

    public CustomerSurvey Pause()
    {
        Status = StatusPaused;
        return this;
    }

    public CustomerSurvey Complete()
    {
        Status = StatusCompleted;
        QueueDomainEvent(new CustomerSurveyCompleted(Id, TotalResponses, AverageScore));
        return this;
    }

    public CustomerSurvey Archive()
    {
        Status = StatusArchived;
        return this;
    }

    public CustomerSurvey RecordResponse(decimal score)
    {
        TotalResponses++;
        // Recalculate average
        AverageScore = AverageScore.HasValue
            ? ((AverageScore.Value * (TotalResponses - 1)) + score) / TotalResponses
            : score;
        return this;
    }

    public CustomerSurvey UpdateNpsScore(int npsScore)
    {
        NpsScore = npsScore;
        return this;
    }

    public CustomerSurvey Update(
        string? title = null,
        string? description = null,
        string? questions = null,
        DateOnly? endDate = null,
        string? targetSegments = null,
        bool? sendReminders = null,
        int? reminderDays = null,
        string? thankYouMessage = null)
    {
        if (title is not null) Title = title;
        if (description is not null) Description = description;
        if (questions is not null) Questions = questions;
        if (endDate.HasValue) EndDate = endDate;
        if (targetSegments is not null) TargetSegments = targetSegments;
        if (sendReminders.HasValue) SendReminders = sendReminders.Value;
        if (reminderDays.HasValue) ReminderDays = reminderDays.Value;
        if (thankYouMessage is not null) ThankYouMessage = thankYouMessage;

        QueueDomainEvent(new CustomerSurveyUpdated(this));
        return this;
    }
}

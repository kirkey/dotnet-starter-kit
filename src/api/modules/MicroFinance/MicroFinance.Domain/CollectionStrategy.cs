using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for CollectionStrategy entity.
/// </summary>
public static class CollectionStrategyConstants
{
    /// <summary>Maximum length for strategy name. (2^7 = 128)</summary>
    public const int NameMaxLength = 128;

    /// <summary>Maximum length for code. (2^5 = 32)</summary>
    public const int CodeMaxLength = 32;

    /// <summary>Maximum length for action type. (2^5 = 32)</summary>
    public const int ActionTypeMaxLength = 32;

    /// <summary>Maximum length for description. (2^10 = 1024)</summary>
    public const int DescriptionMaxLength = 1024;

    /// <summary>Maximum length for message template. (2^11 = 2048)</summary>
    public const int MessageTemplateMaxLength = 2048;
}

/// <summary>
/// Represents a collection strategy rule for automated escalation.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define automated collection workflows based on days past due.
/// - Configure escalation triggers and actions.
/// - Set up SMS, email, and call reminders at specific intervals.
/// - Differentiate collection approaches by loan product or amount.
/// - Enable consistent collection treatment across the portfolio.
/// 
/// Default values and constraints:
/// - Code: Unique strategy identifier (max 32 chars).
/// - Name: Display name for the strategy (max 128 chars).
/// - ActionType: Type of collection action triggered (max 32 chars).
/// - Description: Strategy description (max 1024 chars).
/// - MessageTemplate: Template for automated messages (max 2048 chars).
/// - DaysPastDue: Trigger point for this strategy step.
/// 
/// Business rules:
/// - Defines rules engine for automated collection.
/// - Example: Day 1 → SMS, Day 7 → Call, Day 30 → Field visit.
/// - Strategies vary by loan product, amount, or borrower segment.
/// - LoanProductId null means applies to all products.
/// - Enables consistent treatment across portfolio.
/// </remarks>
public class CollectionStrategy : AuditableEntity, IAggregateRoot
{
    /// <summary>Gets the strategy code.</summary>
    public string Code { get; private set; } = default!;


    /// <summary>Gets the loan product ID (null = applies to all products).</summary>
    public DefaultIdType? LoanProductId { get; private set; }

    /// <summary>Gets the loan product navigation property.</summary>
    public virtual LoanProduct? LoanProduct { get; private set; }

    /// <summary>Gets the minimum days past due to trigger this strategy.</summary>
    public int TriggerDaysPastDue { get; private set; }

    /// <summary>Gets the maximum days past due for this strategy (null = no limit).</summary>
    public int? MaxDaysPastDue { get; private set; }

    /// <summary>Gets the minimum outstanding amount to trigger (null = any amount).</summary>
    public decimal? MinOutstandingAmount { get; private set; }

    /// <summary>Gets the maximum outstanding amount for this strategy (null = no limit).</summary>
    public decimal? MaxOutstandingAmount { get; private set; }

    /// <summary>Gets the action type to perform.</summary>
    public string ActionType { get; private set; } = default!;

    /// <summary>Gets the message template for SMS/Email actions.</summary>
    public string? MessageTemplate { get; private set; }

    /// <summary>Gets the priority order (lower = higher priority).</summary>
    public int Priority { get; private set; }

    /// <summary>Gets the number of days to wait before repeating this action.</summary>
    public int? RepeatIntervalDays { get; private set; }

    /// <summary>Gets the maximum number of times to repeat this action.</summary>
    public int? MaxRepetitions { get; private set; }

    /// <summary>Gets whether to escalate to next strategy if action fails.</summary>
    public bool EscalateOnFailure { get; private set; }

    /// <summary>Gets whether this strategy requires manual approval.</summary>
    public bool RequiresApproval { get; private set; }

    /// <summary>Gets whether the strategy is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the effective start date.</summary>
    public DateOnly? EffectiveFrom { get; private set; }

    /// <summary>Gets the effective end date.</summary>
    public DateOnly? EffectiveTo { get; private set; }

    private CollectionStrategy() { }

    private CollectionStrategy(
        DefaultIdType id,
        string code,
        string name,
        int triggerDaysPastDue,
        string actionType,
        int priority)
    {
        Id = id;
        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        TriggerDaysPastDue = triggerDaysPastDue;
        ActionType = actionType;
        Priority = priority;
        IsActive = true;
        EscalateOnFailure = true;
        RequiresApproval = false;

        QueueDomainEvent(new CollectionStrategyCreated { CollectionStrategy = this });
    }

    /// <summary>Creates a new CollectionStrategy.</summary>
    public static CollectionStrategy Create(
        string code,
        string name,
        int triggerDaysPastDue,
        string actionType,
        int priority)
    {
        return new CollectionStrategy(
            DefaultIdType.NewGuid(),
            code,
            name,
            triggerDaysPastDue,
            actionType,
            priority);
    }

    /// <summary>Updates the strategy.</summary>
    public CollectionStrategy Update(
        string? name = null,
        string? description = null,
        int? triggerDaysPastDue = null,
        int? maxDaysPastDue = null,
        string? actionType = null,
        string? messageTemplate = null,
        int? priority = null)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name.Trim();
        if (description != null) Description = description.Trim();
        if (triggerDaysPastDue.HasValue) TriggerDaysPastDue = triggerDaysPastDue.Value;
        if (maxDaysPastDue.HasValue) MaxDaysPastDue = maxDaysPastDue.Value;
        if (!string.IsNullOrWhiteSpace(actionType)) ActionType = actionType;
        if (messageTemplate != null) MessageTemplate = messageTemplate.Trim();
        if (priority.HasValue) Priority = priority.Value;
        
        return this;
    }

    /// <summary>Sets the amount thresholds.</summary>
    public CollectionStrategy WithAmountThresholds(decimal? minAmount, decimal? maxAmount)
    {
        MinOutstandingAmount = minAmount;
        MaxOutstandingAmount = maxAmount;
        return this;
    }

    /// <summary>Associates with a specific loan product.</summary>
    public CollectionStrategy ForLoanProduct(DefaultIdType loanProductId)
    {
        LoanProductId = loanProductId;
        return this;
    }

    /// <summary>Sets the message template.</summary>
    public CollectionStrategy WithMessageTemplate(string template)
    {
        MessageTemplate = template?.Trim();
        return this;
    }

    /// <summary>Sets repetition settings.</summary>
    public CollectionStrategy WithRepetition(int intervalDays, int maxRepetitions)
    {
        RepeatIntervalDays = intervalDays;
        MaxRepetitions = maxRepetitions;
        return this;
    }

    /// <summary>Sets whether escalation on failure is enabled.</summary>
    public CollectionStrategy WithEscalation(bool escalateOnFailure)
    {
        EscalateOnFailure = escalateOnFailure;
        return this;
    }

    /// <summary>Sets whether approval is required.</summary>
    public CollectionStrategy WithApprovalRequired(bool required)
    {
        RequiresApproval = required;
        return this;
    }

    /// <summary>Sets the effective date range.</summary>
    public CollectionStrategy WithEffectiveDates(DateOnly? from, DateOnly? to)
    {
        EffectiveFrom = from;
        EffectiveTo = to;
        return this;
    }

    /// <summary>Activates the strategy.</summary>
    public CollectionStrategy Activate()
    {
        IsActive = true;
        return this;
    }

    /// <summary>Deactivates the strategy.</summary>
    public CollectionStrategy Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>Checks if this strategy applies to the given conditions.</summary>
    public bool AppliesTo(int daysPastDue, decimal outstandingAmount, DefaultIdType? loanProductId)
    {
        if (!IsActive) return false;
        if (daysPastDue < TriggerDaysPastDue) return false;
        if (MaxDaysPastDue.HasValue && daysPastDue > MaxDaysPastDue.Value) return false;
        if (MinOutstandingAmount.HasValue && outstandingAmount < MinOutstandingAmount.Value) return false;
        if (MaxOutstandingAmount.HasValue && outstandingAmount > MaxOutstandingAmount.Value) return false;
        if (LoanProductId.HasValue && LoanProductId != loanProductId) return false;
        
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (EffectiveFrom.HasValue && today < EffectiveFrom.Value) return false;
        if (EffectiveTo.HasValue && today > EffectiveTo.Value) return false;
        
        return true;
    }
}

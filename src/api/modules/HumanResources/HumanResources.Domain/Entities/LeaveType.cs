using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a type of leave (vacation, sick, personal, etc).
/// Defines accrual rules, carry-forward policies, and approval requirements.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Configurable accrual rates (monthly, annual)
/// - Carry-forward rules (max days, expiry dates)
/// - Approval workflow requirements
/// - Per-company policies (multitenancy support)
/// 
/// Example:
/// - Vacation: 20 days/year, monthly accrual, max carryover 5 days
/// - Sick: 10 days/year, no carryover
/// - Personal: 3 days/year, no carryover
/// </remarks>
public class LeaveType : AuditableEntity, IAggregateRoot
{
    private LeaveType() { }

    private LeaveType(
        DefaultIdType id,
        string leaveName,
        decimal annualAllowance,
        bool isPaid = true,
        bool requiresApproval = true)
    {
        Id = id;
        LeaveName = leaveName;
        AnnualAllowance = annualAllowance;
        IsPaid = isPaid;
        RequiresApproval = requiresApproval;
        IsActive = true;
        AccrualFrequency = "Monthly";
        MaxCarryoverDays = 0;
    }

    /// <summary>
    /// Name of leave type (Vacation, Sick, Personal, etc).
    /// </summary>
    public string LeaveName { get; private set; } = default!;

    /// <summary>
    /// Annual allowance in days.
    /// </summary>
    public decimal AnnualAllowance { get; private set; }

    /// <summary>
    /// Accrual frequency (Monthly, Quarterly, Annual).
    /// </summary>
    public string AccrualFrequency { get; private set; } = default!;

    /// <summary>
    /// Days accrued per frequency period.
    /// </summary>
    public decimal AccrualDaysPerPeriod => AccrualFrequency switch
    {
        "Monthly" => AnnualAllowance / 12,
        "Quarterly" => AnnualAllowance / 4,
        _ => AnnualAllowance
    };

    /// <summary>
    /// Whether leave is paid.
    /// </summary>
    public bool IsPaid { get; private set; }

    /// <summary>
    /// Maximum days that can be carried over to next year.
    /// </summary>
    public decimal MaxCarryoverDays { get; private set; }

    /// <summary>
    /// Whether carried over days expire (null = no expiry).
    /// </summary>
    public int? CarryoverExpiryMonths { get; private set; }

    /// <summary>
    /// Whether manager approval is required.
    /// </summary>
    public bool RequiresApproval { get; private set; }

    /// <summary>
    /// Minimum notice period required in days.
    /// </summary>
    public int? MinimumNoticeDay { get; private set; }

    /// <summary>
    /// Whether this leave type is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Description of the leave type.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Creates a new leave type.
    /// </summary>
    public static LeaveType Create(
        string leaveName,
        decimal annualAllowance,
        bool isPaid = true,
        bool requiresApproval = true)
    {
        if (string.IsNullOrWhiteSpace(leaveName))
            throw new ArgumentException("Leave name is required.", nameof(leaveName));

        if (annualAllowance <= 0)
            throw new ArgumentException("Annual allowance must be greater than 0.", nameof(annualAllowance));

        var leaveType = new LeaveType(
            DefaultIdType.NewGuid(),
            leaveName,
            annualAllowance,
            isPaid,
            requiresApproval);

        return leaveType;
    }

    /// <summary>
    /// Updates leave type configuration.
    /// </summary>
    public LeaveType Update(
        string? leaveName = null,
        decimal? annualAllowance = null,
        string? accrualFrequency = null,
        bool? isPaid = null,
        bool? requiresApproval = null,
        string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(leaveName))
            LeaveName = leaveName;

        if (annualAllowance.HasValue && annualAllowance > 0)
            AnnualAllowance = annualAllowance.Value;

        if (!string.IsNullOrWhiteSpace(accrualFrequency))
            AccrualFrequency = accrualFrequency;

        if (isPaid.HasValue)
            IsPaid = isPaid.Value;

        if (requiresApproval.HasValue)
            RequiresApproval = requiresApproval.Value;

        if (description != null)
            Description = description;

        return this;
    }

    /// <summary>
    /// Sets carryover policy.
    /// </summary>
    public LeaveType SetCarryoverPolicy(decimal maxDays, int? expiryMonths = null)
    {
        if (maxDays < 0)
            throw new ArgumentException("Max carryover days cannot be negative.", nameof(maxDays));

        MaxCarryoverDays = maxDays;
        CarryoverExpiryMonths = expiryMonths;

        return this;
    }

    /// <summary>
    /// Sets minimum notice requirement.
    /// </summary>
    public LeaveType SetMinimumNotice(int days)
    {
        if (days < 0)
            throw new ArgumentException("Minimum notice days cannot be negative.", nameof(days));

        MinimumNoticeDay = days;
        return this;
    }

    /// <summary>
    /// Deactivates the leave type.
    /// </summary>
    public LeaveType Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the leave type.
    /// </summary>
    public LeaveType Activate()
    {
        IsActive = true;
        return this;
    }
}

/// <summary>
/// Leave type constants.
/// </summary>
public static class LeaveTypeNames
{
    public const string Vacation = "Vacation";
    public const string Sick = "Sick";
    public const string Personal = "Personal";
    public const string Bereavement = "Bereavement";
    public const string Maternity = "Maternity";
    public const string Paternity = "Paternity";
    public const string Unpaid = "Unpaid";
}


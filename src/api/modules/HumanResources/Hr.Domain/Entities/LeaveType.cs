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
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the leave name field. (100)
    /// </summary>
    public const int LeaveNameMaxLength = 100;

    /// <summary>
    /// Maximum length for the accrual frequency field. (50)
    /// </summary>
    public const int AccrualFrequencyMaxLength = 50;

    /// <summary>
    /// Maximum length for the leave code field. (50)
    /// </summary>
    public const int LeaveCodeMaxLength = 50;

    /// <summary>
    /// Maximum length for the applicable gender field. (20)
    /// </summary>
    public const int ApplicableGenderMaxLength = 20;

    /// <summary>
    /// Maximum length for the description field. (2^11 = 2048)
    /// </summary>
    public const int DescriptionMaxLength = 2048;

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
    /// Leave classification code per Philippine Labor Code.
    /// Examples: VacationLeave, SickLeave, MaternityLeave, PaternityLeave, SpecialLeave, SoloParentLeave.
    /// Maps to PhilippinesLeaveBenefitsConstants.
    /// </summary>
    public string? LeaveCode { get; private set; }

    /// <summary>
    /// Gender requirement for this leave (Both, Female, Male).
    /// Female: Maternity leave (Art 97, RA 11210) - females only.
    /// Male: Paternity leave (Art 98) - males only.
    /// Both: All employees eligible (vacation, sick, special).
    /// </summary>
    public string ApplicableGender { get; private set; } = "Both";

    /// <summary>
    /// Minimum service days before employee can use this leave.
    /// Example: 30 days for vacation leave, 0 for sick leave.
    /// </summary>
    public int MinimumServiceDays { get; private set; } = 0;

    /// <summary>
    /// Whether medical certificate is required for this leave type.
    /// Example: Sick leave requires medical cert after 3 consecutive days.
    /// </summary>
    public bool RequiresMedicalCertification { get; private set; } = false;

    /// <summary>
    /// Days/threshold after which medical certificate is required.
    /// Example: 3 days for sick leave (after 3 consecutive days, cert required).
    /// </summary>
    public int MedicalCertificateAfterDays { get; private set; } = 0;

    /// <summary>
    /// Whether unused leave can be converted to cash.
    /// Vacation Leave: Yes (convertible at year-end per Labor Code Art 95).
    /// Sick Leave: No (forfeited if unused per Art 96).
    /// </summary>
    public bool IsConvertibleToCash { get; private set; } = false;

    /// <summary>
    /// Whether this leave is cumulative (carries over to next year).
    /// Vacation Leave: Yes (cumulative per Art 95).
    /// Sick Leave: No (non-cumulative per Art 96).
    /// </summary>
    public bool IsCumulative { get; private set; } = false;

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

    /// <summary>
    /// Sets Philippines-specific leave code.
    /// </summary>
    public LeaveType SetLeaveCode(string leaveCode)
    {
        LeaveCode = leaveCode;
        return this;
    }

    /// <summary>
    /// Sets gender applicability (Both, Male, Female).
    /// </summary>
    public LeaveType SetApplicableGender(string gender)
    {
        if (gender != "Both" && gender != "Male" && gender != "Female")
            throw new ArgumentException("Gender must be 'Both', 'Male', or 'Female'.", nameof(gender));

        ApplicableGender = gender;
        return this;
    }

    /// <summary>
    /// Sets minimum service requirement in days.
    /// </summary>
    public LeaveType SetMinimumServiceDays(int days)
    {
        if (days < 0)
            throw new ArgumentException("Minimum service days cannot be negative.", nameof(days));

        MinimumServiceDays = days;
        return this;
    }

    /// <summary>
    /// Sets medical certification requirement.
    /// </summary>
    public LeaveType SetMedicalCertificationRequirement(bool required, int afterDays = 0)
    {
        RequiresMedicalCertification = required;
        MedicalCertificateAfterDays = afterDays;
        return this;
    }

    /// <summary>
    /// Sets whether leave can be converted to cash.
    /// </summary>
    public LeaveType SetCashConvertibility(bool isConvertible)
    {
        IsConvertibleToCash = isConvertible;
        return this;
    }

    /// <summary>
    /// Sets whether leave is cumulative.
    /// </summary>
    public LeaveType SetCumulative(bool isCumulative)
    {
        IsCumulative = isCumulative;
        return this;
    }

    /// <summary>
    /// Checks if employee is eligible for this leave type based on Philippines Labor Code.
    /// </summary>
    public (bool IsEligible, string? Reason) CheckEligibility(
        string? employeeGender,
        DateTime? hireDate,
        DateTime requestDate)
    {
        // Check gender requirement (for maternity/paternity)
        if (ApplicableGender != "Both" && !string.IsNullOrWhiteSpace(employeeGender))
        {
            if (ApplicableGender == "Female" && employeeGender != "Female")
                return (false, $"Leave '{LeaveName}' is only for female employees (Maternity Leave per RA 11210).");

            if (ApplicableGender == "Male" && employeeGender != "Male")
                return (false, $"Leave '{LeaveName}' is only for male employees (Paternity Leave per Art 98).");
        }

        // Check minimum service requirement
        if (hireDate.HasValue && MinimumServiceDays > 0)
        {
            var serviceDays = (requestDate - hireDate.Value).Days;
            if (serviceDays < MinimumServiceDays)
                return (false, $"Employee has {serviceDays} days of service; minimum {MinimumServiceDays} days required.");
        }

        return (true, null);
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


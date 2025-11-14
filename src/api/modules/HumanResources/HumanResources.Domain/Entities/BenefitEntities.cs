using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a benefit offering (health insurance, 401k, etc).
/// Defines benefit types, contribution rules, and eligibility.
/// </summary>
public class Benefit : AuditableEntity, IAggregateRoot
{
    private Benefit() { }

    private Benefit(
        DefaultIdType id,
        string benefitName,
        string benefitType,
        decimal employeeContribution = 0,
        decimal employerContribution = 0)
    {
        Id = id;
        BenefitName = benefitName;
        BenefitType = benefitType;
        EmployeeContribution = employeeContribution;
        EmployerContribution = employerContribution;
        IsActive = true;
        IsRequired = false;
    }

    /// <summary>
    /// Name of the benefit (Health Insurance, 401k, etc).
    /// </summary>
    public string BenefitName { get; private set; } = default!;

    /// <summary>
    /// Type of benefit (Health, Retirement, Life Insurance, etc).
    /// </summary>
    public string BenefitType { get; private set; } = default!;

    /// <summary>
    /// Employee contribution amount/percentage.
    /// </summary>
    public decimal EmployeeContribution { get; private set; }

    /// <summary>
    /// Employer contribution amount/percentage.
    /// </summary>
    public decimal EmployerContribution { get; private set; }

    /// <summary>
    /// Is this benefit required for all employees.
    /// </summary>
    public bool IsRequired { get; private set; }

    /// <summary>
    /// Is this benefit active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Description of the benefit.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Annual coverage limit (if applicable).
    /// </summary>
    public decimal? AnnualLimit { get; private set; }

    /// <summary>
    /// Whether benefit carries over if not used.
    /// </summary>
    public bool IsCarryoverAllowed { get; private set; }

    /// <summary>
    /// Minimum employees eligible for enrollment.
    /// </summary>
    public int? MinimumEligibleEmployees { get; private set; }

    /// <summary>
    /// Pay component ID for payroll deduction.
    /// </summary>
    public DefaultIdType? PayComponentId { get; private set; }

    /// <summary>
    /// Enrollments in this benefit.
    /// </summary>
    public ICollection<BenefitEnrollment> Enrollments { get; private set; } = new List<BenefitEnrollment>();

    /// <summary>
    /// Creates a new benefit.
    /// </summary>
    public static Benefit Create(
        string benefitName,
        string benefitType,
        decimal employeeContribution = 0,
        decimal employerContribution = 0)
    {
        if (string.IsNullOrWhiteSpace(benefitName))
            throw new ArgumentException("Benefit name is required.", nameof(benefitName));

        var benefit = new Benefit(
            DefaultIdType.NewGuid(),
            benefitName,
            benefitType,
            employeeContribution,
            employerContribution);

        return benefit;
    }

    /// <summary>
    /// Updates benefit information.
    /// </summary>
    public Benefit Update(
        string? benefitName = null,
        decimal? employeeContribution = null,
        decimal? employerContribution = null,
        string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(benefitName))
            BenefitName = benefitName;

        if (employeeContribution.HasValue)
            EmployeeContribution = employeeContribution.Value;

        if (employerContribution.HasValue)
            EmployerContribution = employerContribution.Value;

        if (description != null)
            Description = description;

        return this;
    }

    /// <summary>
    /// Makes benefit required for all employees.
    /// </summary>
    public Benefit MakeRequired()
    {
        IsRequired = true;
        return this;
    }

    /// <summary>
    /// Makes benefit optional.
    /// </summary>
    public Benefit MakeOptional()
    {
        IsRequired = false;
        return this;
    }

    /// <summary>
    /// Deactivates the benefit.
    /// </summary>
    public Benefit Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the benefit.
    /// </summary>
    public Benefit Activate()
    {
        IsActive = true;
        return this;
    }
}

/// <summary>
/// Represents employee's enrollment in a benefit.
/// Tracks benefit selection and coverage levels.
/// </summary>
public class BenefitEnrollment : AuditableEntity, IAggregateRoot
{
    private BenefitEnrollment() { }

    private BenefitEnrollment(
        DefaultIdType id,
        DefaultIdType employeeId,
        DefaultIdType benefitId,
        DateTime enrollmentDate,
        DateTime effectiveDate)
    {
        Id = id;
        EmployeeId = employeeId;
        BenefitId = benefitId;
        EnrollmentDate = enrollmentDate;
        EffectiveDate = effectiveDate;
        IsActive = true;

        QueueDomainEvent(new BenefitEnrollmentCreated { Enrollment = this });
    }

    /// <summary>
    /// The employee enrolled in the benefit.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// The benefit enrolled in.
    /// </summary>
    public DefaultIdType BenefitId { get; private set; }
    public Benefit Benefit { get; private set; } = default!;

    /// <summary>
    /// Date of enrollment.
    /// </summary>
    public DateTime EnrollmentDate { get; private set; }

    /// <summary>
    /// Date benefit becomes effective.
    /// </summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>
    /// Coverage level selected (Individual, Family, etc).
    /// </summary>
    public string? CoverageLevel { get; private set; }

    /// <summary>
    /// Employee's contribution amount for this enrollment.
    /// </summary>
    public decimal EmployeeContributionAmount { get; private set; }

    /// <summary>
    /// Employer's contribution amount for this enrollment.
    /// </summary>
    public decimal EmployerContributionAmount { get; private set; }

    /// <summary>
    /// Total annual contribution.
    /// </summary>
    public decimal AnnualContribution => (EmployeeContributionAmount + EmployerContributionAmount) * 12;

    /// <summary>
    /// Date coverage ends (if applicable).
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// Whether enrollment is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Dependent IDs covered by this enrollment (comma-separated).
    /// </summary>
    public string? CoveredDependentIds { get; private set; }

    /// <summary>
    /// Creates a new benefit enrollment.
    /// </summary>
    public static BenefitEnrollment Create(
        DefaultIdType employeeId,
        DefaultIdType benefitId,
        DateTime enrollmentDate,
        DateTime effectiveDate)
    {
        if (effectiveDate < enrollmentDate)
            throw new ArgumentException("Effective date must be after enrollment date.", nameof(effectiveDate));

        var enrollment = new BenefitEnrollment(
            DefaultIdType.NewGuid(),
            employeeId,
            benefitId,
            enrollmentDate,
            effectiveDate);

        return enrollment;
    }

    /// <summary>
    /// Sets coverage level and contribution amounts.
    /// </summary>
    public BenefitEnrollment SetCoverage(
        string coverageLevel,
        decimal employeeContribution,
        decimal employerContribution)
    {
        CoverageLevel = coverageLevel;
        EmployeeContributionAmount = employeeContribution;
        EmployerContributionAmount = employerContribution;

        return this;
    }

    /// <summary>
    /// Adds dependent to enrollment.
    /// </summary>
    public BenefitEnrollment AddDependents(params DefaultIdType[] dependentIds)
    {
        if (dependentIds.Length == 0)
            return this;

        var ids = dependentIds.Select(id => id.ToString()).ToList();
        if (!string.IsNullOrWhiteSpace(CoveredDependentIds))
            ids.AddRange(CoveredDependentIds.Split(','));

        CoveredDependentIds = string.Join(",", ids.Distinct());
        return this;
    }

    /// <summary>
    /// Terminates the enrollment.
    /// </summary>
    public BenefitEnrollment Terminate(DateTime endDate)
    {
        if (endDate < EffectiveDate)
            throw new ArgumentException("End date must be after effective date.", nameof(endDate));

        EndDate = endDate;
        IsActive = false;

        QueueDomainEvent(new BenefitEnrollmentTerminated { Enrollment = this });
        return this;
    }
}

/// <summary>
/// Benefit type constants.
/// </summary>
public static class BenefitType
{
    public const string Health = "Health";
    public const string Dental = "Dental";
    public const string Vision = "Vision";
    public const string Retirement = "Retirement";
    public const string LifeInsurance = "LifeInsurance";
    public const string Disability = "Disability";
    public const string Wellness = "Wellness";
}

/// <summary>
/// Coverage level constants.
/// </summary>
public static class CoverageLevel
{
    public const string Individual = "Individual";
    public const string Employee_Plus_Spouse = "Employee_Plus_Spouse";
    public const string Employee_Plus_Children = "Employee_Plus_Children";
    public const string Family = "Family";
}


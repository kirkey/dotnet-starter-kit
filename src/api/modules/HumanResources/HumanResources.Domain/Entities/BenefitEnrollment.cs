namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

using Events;

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
    /// Allocations/claims/usages tied to this enrollment.
    /// </summary>
    public ICollection<BenefitAllocation> Allocations { get; private set; } = new List<BenefitAllocation>();

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

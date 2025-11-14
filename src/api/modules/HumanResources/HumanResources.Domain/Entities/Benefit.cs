namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a benefit offering for employees.
/// Defines benefit types, contribution rules, and eligibility per Philippines Labor Code Article 100.
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
        IsMandatory = false;
        EffectiveStartDate = DateTime.UtcNow;
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
    /// Is this benefit mandatory per Philippines Labor Code.
    /// </summary>
    public bool IsMandatory { get; private set; }

    /// <summary>
    /// Is this benefit active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Effective start date for the benefit.
    /// </summary>
    public DateTime EffectiveStartDate { get; private set; }

    /// <summary>
    /// Effective end date for the benefit (if applicable).
    /// </summary>
    public DateTime? EffectiveEndDate { get; private set; }

    /// <summary>
    /// Coverage type (Individual, Family, Employee+Spouse, etc).
    /// </summary>
    public string? CoverageType { get; private set; }

    /// <summary>
    /// Provider name (Insurance provider or benefit administrator).
    /// </summary>
    public string? ProviderName { get; private set; }

    /// <summary>
    /// Coverage amount or benefit limit.
    /// </summary>
    public decimal? CoverageAmount { get; private set; }

    /// <summary>
    /// Waiting period in days before benefit is available.
    /// </summary>
    public int? WaitingPeriodDays { get; private set; }

    /// <summary>
    /// Annual coverage limit (if applicable).
    /// </summary>
    public decimal? AnnualLimit { get; private set; }

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
    /// Sets the benefit as mandatory per Philippines Labor Code.
    /// </summary>
    public Benefit SetAsMandatory(bool isMandatory = true)
    {
        IsMandatory = isMandatory;
        return this;
    }

    /// <summary>
    /// Sets effective dates for the benefit.
    /// </summary>
    public Benefit SetEffectiveDates(DateTime startDate, DateTime? endDate = null)
    {
        if (endDate.HasValue && startDate > endDate.Value)
            throw new ArgumentException("Start date must be before end date.");

        EffectiveStartDate = startDate;
        EffectiveEndDate = endDate;
        return this;
    }

    /// <summary>
    /// Sets coverage details.
    /// </summary>
    public Benefit SetCoverageDetails(
        string? coverageType = null,
        decimal? coverageAmount = null,
        string? providerName = null)
    {
        if (!string.IsNullOrWhiteSpace(coverageType))
            CoverageType = coverageType;

        if (coverageAmount.HasValue && coverageAmount.Value > 0)
            CoverageAmount = coverageAmount.Value;

        if (!string.IsNullOrWhiteSpace(providerName))
            ProviderName = providerName;

        return this;
    }

    /// <summary>
    /// Sets the waiting period in days.
    /// </summary>
    public Benefit SetWaitingPeriod(int? days = null)
    {
        if (days.HasValue && days.Value < 0)
            throw new ArgumentException("Waiting period cannot be negative.");

        WaitingPeriodDays = days;
        return this;
    }

    /// <summary>
    /// Sets the benefit description.
    /// </summary>
    public Benefit SetDescription(string? description = null)
    {
        Description = description;
        return this;
    }

    /// <summary>
    /// Updates contribution amounts.
    /// </summary>
    public Benefit UpdateContributions(
        decimal? employeeContribution = null,
        decimal? employerContribution = null)
    {
        if (employeeContribution.HasValue)
        {
            if (employeeContribution.Value < 0)
                throw new ArgumentException("Employee contribution cannot be negative.");
            EmployeeContribution = employeeContribution.Value;
        }

        if (employerContribution.HasValue)
        {
            if (employerContribution.Value < 0)
                throw new ArgumentException("Employer contribution cannot be negative.");
            EmployerContribution = employerContribution.Value;
        }

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

    /// <summary>
    /// Checks if benefit is active on a specified date.
    /// </summary>
    public bool IsActiveOnDate(DateTime date)
    {
        if (!IsActive)
            return false;

        if (date < EffectiveStartDate)
            return false;

        if (EffectiveEndDate.HasValue && date > EffectiveEndDate.Value)
            return false;

        return true;
    }
}

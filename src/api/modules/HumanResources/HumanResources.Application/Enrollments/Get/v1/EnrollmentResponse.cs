namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Get.v1;

/// <summary>
/// Response object for Benefit Enrollment details.
/// </summary>
public sealed record EnrollmentResponse
{
    /// <summary>
    /// Gets the unique identifier of the enrollment.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the employee identifier.
    /// </summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>
    /// Gets the benefit identifier.
    /// </summary>
    public DefaultIdType BenefitId { get; init; }

    /// <summary>
    /// Gets the enrollment date.
    /// </summary>
    public DateTime EnrollmentDate { get; init; }

    /// <summary>
    /// Gets the effective date.
    /// </summary>
    public DateTime EffectiveDate { get; init; }

    /// <summary>
    /// Gets the coverage level selected.
    /// </summary>
    public string? CoverageLevel { get; init; }

    /// <summary>
    /// Gets the employee contribution amount.
    /// </summary>
    public decimal EmployeeContributionAmount { get; init; }

    /// <summary>
    /// Gets the employer contribution amount.
    /// </summary>
    public decimal EmployerContributionAmount { get; init; }

    /// <summary>
    /// Gets the total annual contribution.
    /// </summary>
    public decimal AnnualContribution { get; init; }

    /// <summary>
    /// Gets the end date (if applicable).
    /// </summary>
    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Gets a value indicating whether enrollment is active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets the covered dependent IDs (comma-separated).
    /// </summary>
    public string? CoveredDependentIds { get; init; }
}

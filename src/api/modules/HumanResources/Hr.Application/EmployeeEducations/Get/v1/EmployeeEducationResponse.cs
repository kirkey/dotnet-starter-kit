namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;

/// <summary>
/// Response object for Employee Education details.
/// </summary>
public sealed record EmployeeEducationResponse
{
    /// <summary>
    /// Gets the unique identifier of the education record.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the employee identifier.
    /// </summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>
    /// Gets the education level.
    /// </summary>
    public string EducationLevel { get; init; } = default!;

    /// <summary>
    /// Gets the field of study.
    /// </summary>
    public string FieldOfStudy { get; init; } = default!;

    /// <summary>
    /// Gets the institution name.
    /// </summary>
    public string Institution { get; init; } = default!;

    /// <summary>
    /// Gets the graduation date.
    /// </summary>
    public DateTime GraduationDate { get; init; }

    /// <summary>
    /// Gets the degree name.
    /// </summary>
    public string? Degree { get; init; }

    /// <summary>
    /// Gets the GPA.
    /// </summary>
    public decimal? Gpa { get; init; }

    /// <summary>
    /// Gets the certificate number.
    /// </summary>
    public string? CertificateNumber { get; init; }

    /// <summary>
    /// Gets the certification date.
    /// </summary>
    public DateTime? CertificationDate { get; init; }

    /// <summary>
    /// Gets a value indicating whether education is active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets a value indicating whether education is verified.
    /// </summary>
    public bool IsVerified { get; init; }

    /// <summary>
    /// Gets the verification date.
    /// </summary>
    public DateTime? VerificationDate { get; init; }

    /// <summary>
    /// Gets any notes about the education.
    /// </summary>
    public string? Notes { get; init; }
}


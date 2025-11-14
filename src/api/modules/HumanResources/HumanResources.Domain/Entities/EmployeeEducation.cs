using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents an employee's educational qualification or degree.
/// Tracks education background, institutions, and certification dates.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Multiple education records per employee
/// - Education level: HighSchool, Associate, Bachelor, Master, PhD, Certification, Other
/// - Institution and field of study tracking
/// - Graduation date and GPA
/// - Relevant to job certifications and professional development
/// 
/// Example:
/// - Employee John Doe has:
///   - Bachelor's in Electrical Engineering (2010, GPA 3.8)
///   - Master's in Business Administration (2015, GPA 3.9)
///   - Professional Certification: Project Management (2018)
/// </remarks>
public class EmployeeEducation : AuditableEntity, IAggregateRoot
{
    private EmployeeEducation() { }

    private EmployeeEducation(
        DefaultIdType id,
        DefaultIdType employeeId,
        string educationLevel,
        string fieldOfStudy,
        string institution,
        DateTime graduationDate,
        string? degree = null,
        decimal? gpa = null,
        string? certificateNumber = null,
        DateTime? certificationDate = null)
    {
        Id = id;
        EmployeeId = employeeId;
        EducationLevel = educationLevel;
        FieldOfStudy = fieldOfStudy;
        Institution = institution;
        GraduationDate = graduationDate;
        Degree = degree;
        Gpa = gpa;
        CertificateNumber = certificateNumber;
        CertificationDate = certificationDate;
        IsActive = true;
        IsVerified = false;

        QueueDomainEvent(new EmployeeEducationCreated { Education = this });
    }

    /// <summary>
    /// The employee this education record is associated with.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// Level of education (HighSchool, Associate, Bachelor, Master, PhD, Certification, Other).
    /// </summary>
    public string EducationLevel { get; private set; } = default!;

    /// <summary>
    /// Field of study or major.
    /// </summary>
    public string FieldOfStudy { get; private set; } = default!;

    /// <summary>
    /// Name of the educational institution.
    /// </summary>
    public string Institution { get; private set; } = default!;

    /// <summary>
    /// Date of graduation or completion.
    /// </summary>
    public DateTime GraduationDate { get; private set; }

    /// <summary>
    /// Degree name (e.g., "Bachelor of Science", "Master of Business Administration").
    /// </summary>
    public string? Degree { get; private set; }

    /// <summary>
    /// Grade Point Average (0.0 to 4.0 typically).
    /// </summary>
    public decimal? Gpa { get; private set; }

    /// <summary>
    /// Certificate or license number (for certifications).
    /// </summary>
    public string? CertificateNumber { get; private set; }

    /// <summary>
    /// Date of certification or credentialing.
    /// </summary>
    public DateTime? CertificationDate { get; private set; }

    /// <summary>
    /// Whether education is currently active/valid.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Whether the education has been verified by institution.
    /// </summary>
    public bool IsVerified { get; private set; }

    /// <summary>
    /// Date verification was completed.
    /// </summary>
    public DateTime? VerificationDate { get; private set; }

    /// <summary>
    /// Notes or additional information about the education.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Creates a new employee education record.
    /// </summary>
    public static EmployeeEducation Create(
        DefaultIdType employeeId,
        string educationLevel,
        string fieldOfStudy,
        string institution,
        DateTime graduationDate,
        string? degree = null,
        decimal? gpa = null,
        string? certificateNumber = null,
        DateTime? certificationDate = null)
    {
        if (graduationDate > DateTime.Today)
            throw new ArgumentException("Graduation date cannot be in the future.", nameof(graduationDate));

        if (gpa.HasValue && (gpa < 0 || gpa > 4.0m))
            throw new ArgumentException("GPA must be between 0.0 and 4.0.", nameof(gpa));

        var education = new EmployeeEducation(
            DefaultIdType.NewGuid(),
            employeeId,
            educationLevel,
            fieldOfStudy,
            institution,
            graduationDate,
            degree,
            gpa,
            certificateNumber,
            certificationDate);

        return education;
    }

    /// <summary>
    /// Updates education information.
    /// </summary>
    public EmployeeEducation Update(
        string? fieldOfStudy = null,
        string? degree = null,
        decimal? gpa = null,
        string? notes = null)
    {
        if (!string.IsNullOrWhiteSpace(fieldOfStudy))
            FieldOfStudy = fieldOfStudy;

        if (!string.IsNullOrWhiteSpace(degree))
            Degree = degree;

        if (gpa.HasValue)
            Gpa = gpa;

        if (notes != null)
            Notes = notes;

        QueueDomainEvent(new EmployeeEducationUpdated { Education = this });
        return this;
    }

    /// <summary>
    /// Marks education as verified.
    /// </summary>
    public EmployeeEducation MarkAsVerified()
    {
        IsVerified = true;
        VerificationDate = DateTime.UtcNow;
        QueueDomainEvent(new EmployeeEducationUpdated { Education = this });
        return this;
    }

    /// <summary>
    /// Deactivates this education record.
    /// </summary>
    public EmployeeEducation Deactivate()
    {
        IsActive = false;
        QueueDomainEvent(new EmployeeEducationDeactivated { EducationId = Id });
        return this;
    }

    /// <summary>
    /// Activates this education record.
    /// </summary>
    public EmployeeEducation Activate()
    {
        IsActive = true;
        QueueDomainEvent(new EmployeeEducationActivated { EducationId = Id });
        return this;
    }
}

/// <summary>
/// Education level constants.
/// </summary>
public static class EducationLevel
{
    /// <summary>
    /// High school diploma or equivalent.
    /// </summary>
    public const string HighSchool = "HighSchool";

    /// <summary>
    /// Associate degree (2-year).
    /// </summary>
    public const string Associate = "Associate";

    /// <summary>
    /// Bachelor's degree (4-year).
    /// </summary>
    public const string Bachelor = "Bachelor";

    /// <summary>
    /// Master's degree.
    /// </summary>
    public const string Master = "Master";

    /// <summary>
    /// Doctorate degree (PhD, MD, etc).
    /// </summary>
    public const string Doctorate = "Doctorate";

    /// <summary>
    /// Professional certification or license.
    /// </summary>
    public const string Certification = "Certification";

    /// <summary>
    /// Other or unspecified education.
    /// </summary>
    public const string Other = "Other";
}


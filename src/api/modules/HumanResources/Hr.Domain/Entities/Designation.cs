using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a job designation with area-specific salary ranges and classifications.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Designations represent job titles with area-specific salary configurations
/// - Area field determines geographic region (Metro Manila, Visayas, Mindanao, Luzon, National)
/// - Salary grades (Grade 1-5, Executive) define career progression levels
/// - SalaryRange per designation varies by area due to cost-of-living differences
/// - IsManagerial flag indicates leadership positions for org structure
/// 
/// Example:
/// - Designation "Senior Engineer" in Metro Manila: Grade 3, ₱80K-120K, IsManagerial=false
/// - Designation "Senior Engineer" in Visayas: Grade 3, ₱65K-100K, IsManagerial=false
/// - Designation "Engineering Manager" in Metro Manila: Grade 5, ₱120K-180K, IsManagerial=true
/// </remarks>
public class Designation : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the designation code field. (50)
    /// </summary>
    public const int CodeMaxLength = 50;

    /// <summary>
    /// Maximum length for the designation title field. (2^8 = 256)
    /// </summary>
    public const int TitleMaxLength = 256;

    /// <summary>
    /// Maximum length for the area field. (100)
    /// </summary>
    public const int AreaMaxLength = 100;

    /// <summary>
    /// Maximum length for the salary grade field. (50)
    /// </summary>
    public const int SalaryGradeMaxLength = 50;

    /// <summary>
    /// Maximum length for the description field. (2^11 = 2048)
    /// </summary>
    public const int DescriptionMaxLength = 2048;

    private Designation() { }

    private Designation(
        DefaultIdType id,
        string code,
        string title,
        string? area,
        string? description)
    {
        Id = id;
        Code = code;
        Title = title;
        Area = area ?? "National";
        Description = description;
        IsActive = true;
        IsManagerial = false;

        QueueDomainEvent(new DesignationCreated { Designation = this });
    }

    /// <summary>
    /// Unique code for this designation (e.g., "ENG-001", "MGR-001").
    /// Must be unique across the system.
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Job title/designation title (e.g., "Senior Software Engineer", "Area Manager").
    /// </summary>
    public string Title { get; private set; } = default!;

    /// <summary>
    /// Geographic area/region for this designation (Metro Manila, Visayas, Mindanao, Luzon, National).
    /// Determines applicable salary ranges and cost-of-living adjustments.
    /// </summary>
    public string Area { get; private set; } = "National";

    /// <summary>
    /// Salary grade classification (Grade 1-5, Executive) for compensation comparison and career progression.
    /// Used to show career advancement paths and market positioning.
    /// </summary>
    public string? SalaryGrade { get; private set; }

    /// <summary>
    /// Detailed job description including responsibilities and requirements.
    /// Can vary per area and designation.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Minimum salary for this designation in the specified area.
    /// Reflects hiring base rate considering market conditions and cost of living.
    /// </summary>
    public decimal? MinimumSalary { get; private set; }

    /// <summary>
    /// Maximum salary for this designation in the specified area.
    /// Reflects top of range for experienced employees in this designation.
    /// </summary>
    public decimal? MaximumSalary { get; private set; }

    /// <summary>
    /// Whether this designation is currently active.
    /// Only active designations can be assigned to employees.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Whether this is a managerial/leadership position.
    /// Used for organizational reporting structure and hierarchy visualization.
    /// Affects eligibility for certain policies and permissions.
    /// </summary>
    public bool IsManagerial { get; private set; }

    /// <summary>
    /// Creates a new designation with area-specific salary configuration.
    /// </summary>
    public static Designation Create(
        string code,
        string title,
        string? area = "National",
        string? description = null,
        string? salaryGrade = null,
        decimal? minimumSalary = null,
        decimal? maximumSalary = null,
        bool isManagerial = false)
    {
        var designation = new Designation(
            DefaultIdType.NewGuid(),
            code,
            title,
            area,
            description)
        {
            SalaryGrade = salaryGrade,
            MinimumSalary = minimumSalary,
            MaximumSalary = maximumSalary,
            IsManagerial = isManagerial
        };

        return designation;
    }

    /// <summary>
    /// Updates designation information.
    /// </summary>
    public Designation Update(
        string? title,
        string? area,
        string? description,
        string? salaryGrade,
        decimal? minimumSalary,
        decimal? maximumSalary,
        bool? isManagerial,
        bool? isActive)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(title) && !string.Equals(Title, title, StringComparison.OrdinalIgnoreCase))
        {
            Title = title;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(area) && !string.Equals(Area, area, StringComparison.OrdinalIgnoreCase))
        {
            Area = area;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.Equals(SalaryGrade, salaryGrade, StringComparison.OrdinalIgnoreCase))
        {
            SalaryGrade = salaryGrade;
            isUpdated = true;
        }

        if (MinimumSalary != minimumSalary)
        {
            MinimumSalary = minimumSalary;
            isUpdated = true;
        }

        if (MaximumSalary != maximumSalary)
        {
            MaximumSalary = maximumSalary;
            isUpdated = true;
        }

        if (isManagerial.HasValue && IsManagerial != isManagerial.Value)
        {
            IsManagerial = isManagerial.Value;
            isUpdated = true;
        }

        if (isActive.HasValue && IsActive != isActive.Value)
        {
            IsActive = isActive.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new DesignationUpdated { Designation = this });
        }

        return this;
    }

    /// <summary>
    /// Activates this designation for employee assignment.
    /// </summary>
    public Designation Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new DesignationUpdated { Designation = this });
        }

        return this;
    }

    /// <summary>
    /// Deactivates this designation, preventing new employee assignments.
    /// Preserves historical data for reporting.
    /// </summary>
    public Designation Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new DesignationUpdated { Designation = this });
        }

        return this;
    }
}

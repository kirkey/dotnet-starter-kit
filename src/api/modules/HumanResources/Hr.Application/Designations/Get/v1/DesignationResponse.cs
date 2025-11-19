namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;

/// <summary>
/// Response for designation details.
/// Designations represent job titles with area-specific salary ranges and classifications.
/// </summary>
public sealed record DesignationResponse
{
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Unique designation code (e.g., "ENG-001", "MGR-001").
    /// </summary>
    public string Code { get; init; } = default!;
    
    /// <summary>
    /// Job title/designation title (e.g., "Senior Software Engineer").
    /// </summary>
    public string Title { get; init; } = default!;
    
    /// <summary>
    /// Geographic area/region for this designation (Metro Manila, Visayas, Mindanao, Luzon, National).
    /// Determines applicable salary ranges and cost-of-living adjustments.
    /// </summary>
    public string? Area { get; init; }
    
    /// <summary>
    /// Salary grade classification (Grade 1-5, Executive) for compensation comparison and career progression.
    /// </summary>
    public string? SalaryGrade { get; init; }
    
    /// <summary>
    /// Detailed job description including responsibilities and requirements.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Minimum salary for this designation in the specified area.
    /// Reflects hiring base rate and market considerations.
    /// </summary>
    public decimal? MinimumSalary { get; init; }
    
    /// <summary>
    /// Maximum salary for this designation in the specified area.
    /// Reflects top of range for experienced employees.
    /// </summary>
    public decimal? MaximumSalary { get; init; }
    
    /// <summary>
    /// Whether the designation is active and available for employee assignments.
    /// </summary>
    public bool IsActive { get; init; }
    
    /// <summary>
    /// Whether this is a managerial/leadership position.
    /// Used for organizational reporting structure and hierarchy visualization.
    /// </summary>
    public bool IsManagerial { get; init; }
}


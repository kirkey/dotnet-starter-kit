using FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Search.v1;

/// <summary>
/// Request to search and filter designations.
/// Supports filtering by area, salary grade, status, and salary ranges.
/// </summary>
public class SearchDesignationsRequest : PaginationFilter, IRequest<PagedList<DesignationResponse>>
{
    /// <summary>
    /// Filter by geographic area (Metro Manila, Visayas, Mindanao, Luzon, National).
    /// </summary>
    public string? Area { get; set; }
    
    /// <summary>
    /// Filter by salary grade (Grade 1-5, Executive).
    /// </summary>
    public string? SalaryGrade { get; set; }
    
    /// <summary>
    /// Filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }
    
    /// <summary>
    /// Filter by managerial status.
    /// </summary>
    public bool? IsManagerial { get; set; }
    
    /// <summary>
    /// Filter by minimum salary range (lower bound).
    /// </summary>
    public decimal? SalaryMin { get; set; }
    
    /// <summary>
    /// Filter by maximum salary range (upper bound).
    /// </summary>
    public decimal? SalaryMax { get; set; }
}


namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Search.v1;

/// <summary>
/// Request to search employee designation history with temporal queries.
/// </summary>
public class SearchEmployeeHistoryRequest : PaginationFilter, IRequest<PagedList<EmployeeHistoryDto>>
{
    /// <summary>
    /// Organizational unit ID to filter employees
    /// </summary>
    public DefaultIdType? OrganizationalUnitId { get; set; }

    /// <summary>
    /// Designation ID to find all employees who held it
    /// </summary>
    public DefaultIdType? DesignationId { get; set; }

    /// <summary>
    /// Point-in-time query: Get employees active on this date
    /// </summary>
    public DateTime? PointInTimeDate { get; set; }

    /// <summary>
    /// Show acting designations too
    /// </summary>
    public bool IncludeActingDesignations { get; set; } = false;

    /// <summary>
    /// Employment status filter
    /// </summary>
    public string? EmploymentStatus { get; set; }

    /// <summary>
    /// Date range: From
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Date range: To
    /// </summary>
    public DateTime? ToDate { get; set; }
}

/// <summary>
/// DTO for employee designation history display
/// </summary>
public sealed record EmployeeHistoryDto
{
    public DefaultIdType EmployeeId { get; init; }
    public string EmployeeNumber { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public string CurrentDesignation { get; init; } = default!;
    public DateTime? CurrentDesignationStart { get; init; }
    public string OrganizationalUnitName { get; init; } = default!;
    public int TotalDesignationChanges { get; init; }
    public List<DesignationHistoryDto> DesignationHistory { get; init; } = new();
}

/// <summary>
/// DTO for individual designation history entry
/// </summary>
public sealed record DesignationHistoryDto
{
    public string Designation { get; init; } = default!;
    public DateTime EffectiveDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int TenureMonths { get; init; }
    public bool IsPlantilla { get; init; }
    public bool IsActingAs { get; init; }
    public string Status => EndDate.HasValue ? "Previous" : "Current";
}


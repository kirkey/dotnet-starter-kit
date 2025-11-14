namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Search.v1;

/// <summary>
/// Request to search attendance records with filtering and pagination.
/// </summary>
public class SearchAttendanceRequest : PaginationFilter, IRequest<PagedList<Get.v1.AttendanceResponse>>
{
    /// <summary>
    /// Gets or sets the employee ID to filter attendance for a specific employee.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the start date for filtering.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date for filtering.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the attendance status filter (Present, Late, Absent, etc.).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by approval status.
    /// </summary>
    public bool? IsApproved { get; set; }
}

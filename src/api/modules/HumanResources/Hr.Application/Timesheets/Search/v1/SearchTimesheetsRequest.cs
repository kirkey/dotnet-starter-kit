namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Search.v1;

/// <summary>
/// Request to search timesheets with filtering and pagination.
/// </summary>
public class SearchTimesheetsRequest : PaginationFilter, IRequest<PagedList<Get.v1.TimesheetResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
    public bool? IsApproved { get; set; }
}


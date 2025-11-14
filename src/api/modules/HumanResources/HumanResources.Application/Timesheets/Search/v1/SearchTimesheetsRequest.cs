using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Search.v1;

public class SearchTimesheetsRequest : PaginationFilter, IRequest<PagedList<TimesheetResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
    public bool? IsApproved { get; set; }
}


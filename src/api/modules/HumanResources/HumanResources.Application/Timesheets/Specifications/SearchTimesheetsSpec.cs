using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Specifications;

public class SearchTimesheetsSpec : EntitiesByPaginationFilterSpec<Timesheet, TimesheetResponse>
{
    public SearchTimesheetsSpec(SearchTimesheetsRequest request)
        : base(request) =>
        Query
            .Where(t => t.EmployeeId == request.EmployeeId, request.EmployeeId.HasValue)
            .Where(t => t.StartDate >= request.FromDate, request.FromDate.HasValue)
            .Where(t => t.EndDate <= request.ToDate, request.ToDate.HasValue)
            .Where(t => t.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(t => t.PeriodType == request.PeriodType, !string.IsNullOrWhiteSpace(request.PeriodType))
            .Where(t => t.IsApproved == request.IsApproved, request.IsApproved.HasValue)
            .OrderByDescending(t => t.StartDate, !request.HasOrderBy());
}


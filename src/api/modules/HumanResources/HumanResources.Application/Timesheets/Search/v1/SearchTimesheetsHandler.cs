using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Search.v1;

public sealed class SearchTimesheetsHandler(
    [FromKeyedServices("hr:timesheets")] IReadRepository<Timesheet> repository)
    : IRequestHandler<SearchTimesheetsRequest, PagedList<TimesheetResponse>>
{
    public async Task<PagedList<TimesheetResponse>> Handle(
        SearchTimesheetsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchTimesheetsSpec(request);
        var timesheets = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = timesheets.Select(ts => new TimesheetResponse
        {
            Id = ts.Id,
            EmployeeId = ts.EmployeeId,
            StartDate = ts.StartDate,
            EndDate = ts.EndDate,
            PeriodType = ts.PeriodType,
            RegularHours = ts.RegularHours,
            OvertimeHours = ts.OvertimeHours,
            TotalHours = ts.TotalHours,
            Status = ts.Status,
            ApproverManagerId = ts.ApproverManagerId,
            SubmittedDate = ts.SubmittedDate,
            ApprovedDate = ts.ApprovedDate,
            IsLocked = ts.IsLocked,
            IsApproved = ts.IsApproved
        }).ToList();

        return new PagedList<TimesheetResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

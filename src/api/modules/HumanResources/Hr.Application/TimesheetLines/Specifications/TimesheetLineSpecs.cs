using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Specifications;

/// <summary>
/// Specification to get a timesheet line by timesheet ID and work date.
/// </summary>
public sealed class TimesheetLineByTimesheetAndDateSpec : SingleResultSpecification<TimesheetLine>
{
    public TimesheetLineByTimesheetAndDateSpec(DefaultIdType timesheetId, DateTime workDate)
    {
        Query.Where(x => x.TimesheetId == timesheetId && x.WorkDate.Date == workDate.Date);
    }
}

/// <summary>
/// Specification for searching timesheet lines with filters.
/// </summary>
public sealed class TimesheetLineSearchSpec : EntitiesByPaginationFilterSpec<TimesheetLine, TimesheetLine>
{
    public TimesheetLineSearchSpec(SearchTimesheetLinesRequest request)
        : base(request)
    {
        if (request.TimesheetId.HasValue)
            Query.Where(x => x.TimesheetId == request.TimesheetId.Value);

        if (request.WorkDate.HasValue)
            Query.Where(x => x.WorkDate.Date == request.WorkDate.Value.Date);

        if (request.FromDate.HasValue)
            Query.Where(x => x.WorkDate.Date >= request.FromDate.Value.Date);

        if (request.ToDate.HasValue)
            Query.Where(x => x.WorkDate.Date <= request.ToDate.Value.Date);

        if (!string.IsNullOrWhiteSpace(request.ProjectId))
            Query.Where(x => x.ProjectId == request.ProjectId);

        if (request.IsBillable.HasValue)
            Query.Where(x => x.IsBillable == request.IsBillable.Value);

        Query.OrderByDescending(x => x.WorkDate);
    }
}


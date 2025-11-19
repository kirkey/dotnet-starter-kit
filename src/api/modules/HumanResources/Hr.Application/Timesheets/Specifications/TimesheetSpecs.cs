namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Specifications;

public class TimesheetByIdSpec : Specification<Timesheet>, ISingleResultSpecification<Timesheet>
{
    public TimesheetByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee);
    }
}

public class SearchTimesheetsSpec : Specification<Timesheet>
{
    public SearchTimesheetsSpec(Search.v1.SearchTimesheetsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .OrderByDescending(x => x.EndDate);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.StartDate.HasValue)
            Query.Where(x => x.EndDate >= request.StartDate);

        if (request.EndDate.HasValue)
            Query.Where(x => x.StartDate <= request.EndDate);

        if (!string.IsNullOrWhiteSpace(request.Status))
            Query.Where(x => x.Status == request.Status);

        if (request.IsApproved.HasValue)
            Query.Where(x => x.IsApproved == request.IsApproved);
    }
}


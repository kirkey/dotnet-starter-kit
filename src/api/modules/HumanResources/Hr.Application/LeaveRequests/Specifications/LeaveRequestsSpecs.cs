namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Specifications;

public class LeaveRequestByIdSpec : Specification<LeaveRequest>, ISingleResultSpecification<LeaveRequest>
{
    public LeaveRequestByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee)
            .Include(x => x.LeaveType);
    }
}

public class SearchLeaveRequestsSpec : Specification<LeaveRequest>
{
    public SearchLeaveRequestsSpec(Search.v1.SearchLeaveRequestsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .Include(x => x.LeaveType)
            .OrderByDescending(x => x.StartDate);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.LeaveTypeId.HasValue)
            Query.Where(x => x.LeaveTypeId == request.LeaveTypeId);

        if (!string.IsNullOrWhiteSpace(request.Status))
            Query.Where(x => x.Status == request.Status);

        if (request.StartDate.HasValue)
            Query.Where(x => x.EndDate >= request.StartDate);

        if (request.EndDate.HasValue)
            Query.Where(x => x.StartDate <= request.EndDate);
    }
}


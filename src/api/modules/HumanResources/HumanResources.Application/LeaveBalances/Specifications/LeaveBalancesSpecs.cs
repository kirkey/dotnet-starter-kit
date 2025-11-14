namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Specifications;

public class LeaveBalanceByIdSpec : Specification<LeaveBalance>, ISingleResultSpecification<LeaveBalance>
{
    public LeaveBalanceByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee)
            .Include(x => x.LeaveType);
    }
}

public class SearchLeaveBalancesSpec : Specification<LeaveBalance>
{
    public SearchLeaveBalancesSpec(Search.v1.SearchLeaveBalancesRequest request)
    {
        Query
            .Include(x => x.Employee)
            .Include(x => x.LeaveType)
            .OrderByDescending(x => x.Year)
            .ThenBy(x => x.EmployeeId);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.LeaveTypeId.HasValue)
            Query.Where(x => x.LeaveTypeId == request.LeaveTypeId);

        if (request.Year.HasValue)
            Query.Where(x => x.Year == request.Year);
    }
}


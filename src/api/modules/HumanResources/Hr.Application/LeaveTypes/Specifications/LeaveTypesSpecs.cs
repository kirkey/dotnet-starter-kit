namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Specifications;

public class LeaveTypeByIdSpec : Specification<LeaveType>, ISingleResultSpecification<LeaveType>
{
    public LeaveTypeByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

public class SearchLeaveTypesSpec : Specification<LeaveType>
{
    public SearchLeaveTypesSpec(Search.v1.SearchLeaveTypesRequest request)
    {
        Query.OrderBy(x => x.LeaveName);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            Query.Where(x => x.LeaveName.Contains(request.SearchString) ||
                           x.Description!.Contains(request.SearchString));

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);

        if (request.IsPaid.HasValue)
            Query.Where(x => x.IsPaid == request.IsPaid);
    }
}


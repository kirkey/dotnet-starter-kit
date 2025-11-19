namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Specifications;

using Req = Search.v1.SearchShiftAssignmentsRequest;

/// <summary>
/// Specification for searching shift assignments with pagination and filters.
/// </summary>
public sealed class ShiftAssignmentSearchSpec : EntitiesByPaginationFilterSpec<ShiftAssignment, ShiftAssignment>
{
    public ShiftAssignmentSearchSpec(Req request)
        : base(request)
    {
        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId.Value);

        if (request.ShiftId.HasValue)
            Query.Where(x => x.ShiftId == request.ShiftId.Value);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive.Value);

        if (request.IsRecurring.HasValue)
            Query.Where(x => x.IsRecurring == request.IsRecurring.Value);

        Query.OrderByDescending(x => x.StartDate);
    }
}


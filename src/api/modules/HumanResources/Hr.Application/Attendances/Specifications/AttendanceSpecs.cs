namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Specifications;

/// <summary>
/// Specification for getting an attendance record by ID.
/// </summary>
public class AttendanceByIdSpec : Specification<Attendance>, ISingleResultSpecification<Attendance>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AttendanceByIdSpec"/> class.
    /// </summary>
    public AttendanceByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for searching attendance records with filters.
/// </summary>
public class SearchAttendanceSpec : Specification<Attendance>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchAttendanceSpec"/> class.
    /// </summary>
    public SearchAttendanceSpec(Search.v1.SearchAttendanceRequest request)
    {
        Query
            .Include(x => x.Employee)
            .OrderByDescending(x => x.AttendanceDate)
            .ThenBy(x => x.EmployeeId);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.StartDate.HasValue)
            Query.Where(x => x.AttendanceDate >= request.StartDate);

        if (request.EndDate.HasValue)
            Query.Where(x => x.AttendanceDate <= request.EndDate);

        if (!string.IsNullOrWhiteSpace(request.Status))
            Query.Where(x => x.Status == request.Status);

        if (request.IsApproved.HasValue)
            Query.Where(x => x.IsApproved == request.IsApproved);
    }
}


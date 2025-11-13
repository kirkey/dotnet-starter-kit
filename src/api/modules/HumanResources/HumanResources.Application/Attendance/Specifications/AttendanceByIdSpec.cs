namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Specifications;

public class AttendanceByIdSpec : Specification<Domain.Entities.Attendance>
{
    public AttendanceByIdSpec(DefaultIdType id)
    {
        Query.Where(a => a.Id == id);
    }
}


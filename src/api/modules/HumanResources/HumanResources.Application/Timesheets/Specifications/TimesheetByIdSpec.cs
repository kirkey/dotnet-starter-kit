namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Specifications;

public class TimesheetByIdSpec : Specification<Domain.Entities.Timesheet>
{
    public TimesheetByIdSpec(DefaultIdType id)
    {
        Query.Where(t => t.Id == id);
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Specifications;

public class TimesheetByIdSpec : Specification<Timesheet>
{
    public TimesheetByIdSpec(DefaultIdType id)
    {
        Query.Where(t => t.Id == id);
    }
}


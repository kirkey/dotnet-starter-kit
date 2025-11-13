namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Specifications;

public class ShiftByIdSpec : Specification<Shift>
{
    public ShiftByIdSpec(DefaultIdType id)
    {
        Query.Where(s => s.Id == id);
    }
}


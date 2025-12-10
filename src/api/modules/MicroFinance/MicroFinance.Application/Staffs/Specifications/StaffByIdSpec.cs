namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Specifications;

public sealed class StaffByIdSpec : Specification<Staff>, ISingleResultSpecification<Staff>
{
    public StaffByIdSpec(DefaultIdType id) => Query.Where(x => x.Id == id);
}

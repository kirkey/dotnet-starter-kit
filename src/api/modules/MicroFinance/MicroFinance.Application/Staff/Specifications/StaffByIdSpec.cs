using Ardalis.Specification;
using StaffEntity = FSH.Starter.WebApi.MicroFinance.Domain.Staff;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Specifications;

public sealed class StaffByIdSpec : Specification<StaffEntity>, ISingleResultSpecification<StaffEntity>
{
    public StaffByIdSpec(Guid id) => Query.Where(x => x.Id == id);
}

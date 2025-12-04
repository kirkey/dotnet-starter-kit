using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Get.v1;

public sealed class CustomerSegmentByIdSpec : Specification<CustomerSegment>, ISingleResultSpecification<CustomerSegment>
{
    public CustomerSegmentByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}

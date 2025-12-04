using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Specifications;

public sealed class GroupMembershipByIdSpec : Specification<GroupMembership>, ISingleResultSpecification<GroupMembership>
{
    public GroupMembershipByIdSpec(Guid id)
    {
        Query.Where(gm => gm.Id == id);
    }
}

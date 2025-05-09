using Ardalis.Specification;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Features.Dtos;
using FSH.Starter.WebApi.App.Features.GetList.v1;

namespace FSH.Starter.WebApi.App.Features.Queries;

public class GroupByNameSpec : Specification<Group, GroupDto>, ISingleResultSpecification<Group, GroupDto>
{
    public GroupByNameSpec(string name)
    {
        Query.Where(x => x.Name.Equals(name));
    }
}

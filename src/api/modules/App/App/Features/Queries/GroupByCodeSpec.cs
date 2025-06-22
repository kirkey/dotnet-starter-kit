using Ardalis.Specification;
using FSH.Starter.WebApi.App.Domain;
using FSH.Starter.WebApi.App.Features.Dtos;

namespace FSH.Starter.WebApi.App.Features.Queries;

public class GroupByCodeSpec : Specification<Group, GroupDto>, ISingleResultSpecification<Group, GroupDto>
{
    public GroupByCodeSpec(string code)
    {
        Query.Where(x => x.Code.Equals(code));
    }
}

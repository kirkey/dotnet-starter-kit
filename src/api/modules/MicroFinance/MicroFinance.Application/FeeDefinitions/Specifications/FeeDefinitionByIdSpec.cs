using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Specifications;

public sealed class FeeDefinitionByIdSpec : Specification<FeeDefinition>, ISingleResultSpecification<FeeDefinition>
{
    public FeeDefinitionByIdSpec(DefaultIdType id)
    {
        Query.Where(fd => fd.Id == id);
    }
}

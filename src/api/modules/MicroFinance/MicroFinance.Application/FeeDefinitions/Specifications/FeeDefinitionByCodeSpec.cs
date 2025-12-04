using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Specifications;

public sealed class FeeDefinitionByCodeSpec : Specification<FeeDefinition>, ISingleResultSpecification<FeeDefinition>
{
    public FeeDefinitionByCodeSpec(string code)
    {
        Query.Where(fd => fd.Code == code);
    }
}

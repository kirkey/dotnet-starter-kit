namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Specifications;

using Ardalis.Specification;
using Domain.Entities;

/// <summary>
/// Specification for getting benefit by ID.
/// </summary>
public sealed class BenefitByIdSpec : Specification<Benefit>, ISingleResultSpecification<Benefit>
{
    public BenefitByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}


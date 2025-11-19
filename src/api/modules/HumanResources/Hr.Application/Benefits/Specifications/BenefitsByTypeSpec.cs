namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Specifications;

using Ardalis.Specification;
using Domain.Entities;

/// <summary>
/// Specification for getting benefits by type.
/// </summary>
public sealed class BenefitsByTypeSpec : Specification<Benefit>
{
    public BenefitsByTypeSpec(string benefitType)
    {
        Query.Where(x => x.BenefitType == benefitType && x.IsActive)
            .OrderBy(x => x.BenefitName);
    }
}


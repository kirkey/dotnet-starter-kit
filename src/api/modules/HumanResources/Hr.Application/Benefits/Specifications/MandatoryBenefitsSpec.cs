namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Specifications;

using Ardalis.Specification;
using Domain.Entities;

/// <summary>
/// Specification for getting mandatory benefits.
/// </summary>
public sealed class MandatoryBenefitsSpec : Specification<Benefit>
{
    public MandatoryBenefitsSpec()
    {
        Query.Where(x => x.IsMandatory && x.IsActive)
            .OrderBy(x => x.BenefitName);
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Specification for getting benefit enrollment by ID.
/// </summary>
public sealed class BenefitEnrollmentByIdSpec : Specification<BenefitEnrollment>, ISingleResultSpecification<BenefitEnrollment>
{
    public BenefitEnrollmentByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Employee)
            .Include(x => x.Benefit);
    }
}


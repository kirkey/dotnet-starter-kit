using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Specifications;

/// <summary>
/// Specification for retrieving an insurance policy by ID.
/// </summary>
public sealed class InsurancePolicyByIdSpec : Specification<InsurancePolicy>, ISingleResultSpecification<InsurancePolicy>
{
    public InsurancePolicyByIdSpec(Guid id)
    {
        Query.Where(p => p.Id == id);
    }
}

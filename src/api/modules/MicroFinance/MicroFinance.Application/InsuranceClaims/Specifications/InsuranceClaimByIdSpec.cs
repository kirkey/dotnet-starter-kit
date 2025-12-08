using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Specifications;

/// <summary>
/// Specification for retrieving an insurance claim by ID.
/// </summary>
public sealed class InsuranceClaimByIdSpec : Specification<InsuranceClaim>, ISingleResultSpecification<InsuranceClaim>
{
    public InsuranceClaimByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}

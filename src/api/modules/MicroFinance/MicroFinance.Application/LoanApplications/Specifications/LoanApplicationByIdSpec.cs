using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Specifications;

/// <summary>
/// Specification for retrieving a loan application by ID.
/// </summary>
public sealed class LoanApplicationByIdSpec : Specification<LoanApplication>, ISingleResultSpecification<LoanApplication>
{
    public LoanApplicationByIdSpec(Guid id)
    {
        Query.Where(a => a.Id == id);
    }
}

using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Get.v1;

/// <summary>
/// Specification to get a loan officer target by ID.
/// </summary>
public sealed class LoanOfficerTargetByIdSpec : Specification<LoanOfficerTarget>, ISingleResultSpecification<LoanOfficerTarget>
{
    public LoanOfficerTargetByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}

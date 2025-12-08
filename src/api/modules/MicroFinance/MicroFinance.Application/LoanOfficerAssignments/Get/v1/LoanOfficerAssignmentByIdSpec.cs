using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Get.v1;

public sealed class LoanOfficerAssignmentByIdSpec : Specification<LoanOfficerAssignment>, ISingleResultSpecification<LoanOfficerAssignment>
{
    public LoanOfficerAssignmentByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

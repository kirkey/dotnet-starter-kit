using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;

public sealed class LoansByMemberIdSpec : Specification<Loan>
{
    public LoansByMemberIdSpec(DefaultIdType memberId, bool? isActive = null)
    {
        Query.Where(l => l.MemberId == memberId);

        if (isActive.HasValue && isActive.Value)
        {
            Query.Where(l => l.Status == "DISBURSED" || l.Status == "APPROVED" || l.Status == "PENDING");
        }
    }
}

using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;

public sealed class LoanByIdSpec : Specification<Loan>, ISingleResultSpecification<Loan>
{
    public LoanByIdSpec(DefaultIdType id)
    {
        Query.Where(l => l.Id == id)
            .Include(l => l.Member)
            .Include(l => l.LoanProduct)
            .Include(l => l.LoanSchedules)
            .Include(l => l.LoanRepayments)
            .Include(l => l.LoanGuarantors)
            .Include(l => l.LoanCollaterals);
    }
}

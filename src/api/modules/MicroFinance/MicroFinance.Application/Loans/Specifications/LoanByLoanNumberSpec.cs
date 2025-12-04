using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;

public sealed class LoanByLoanNumberSpec : Specification<Loan>, ISingleResultSpecification<Loan>
{
    public LoanByLoanNumberSpec(string loanNumber)
    {
        Query.Where(l => l.LoanNumber == loanNumber);
    }
}

using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Specifications;

/// <summary>
/// Specification to get a loan repayment by ID with related data.
/// </summary>
public class LoanRepaymentByIdSpec : Specification<LoanRepayment>
{
    public LoanRepaymentByIdSpec(Guid id)
    {
        Query
            .Where(r => r.Id == id)
            .Include(r => r.Loan)
            .ThenInclude(l => l.Member);
    }
}

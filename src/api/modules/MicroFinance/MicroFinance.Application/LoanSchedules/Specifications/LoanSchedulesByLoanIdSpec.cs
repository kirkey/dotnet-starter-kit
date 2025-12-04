using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Specifications;

public sealed class LoanSchedulesByLoanIdSpec : Specification<LoanSchedule>
{
    public LoanSchedulesByLoanIdSpec(Guid loanId)
    {
        Query.Where(ls => ls.LoanId == loanId)
             .OrderBy(ls => ls.InstallmentNumber);
    }
}

using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Specifications;

public sealed class LoanScheduleByIdSpec : Specification<LoanSchedule>, ISingleResultSpecification<LoanSchedule>
{
    public LoanScheduleByIdSpec(DefaultIdType id)
    {
        Query.Where(ls => ls.Id == id);
    }
}

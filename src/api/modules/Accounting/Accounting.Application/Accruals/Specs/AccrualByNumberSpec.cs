using Accounting.Domain.Entities;

namespace Accounting.Application.Accruals.Specs;

public sealed class AccrualByNumberSpec : Specification<Accrual>, ISingleResultSpecification<Accrual>
{
    public AccrualByNumberSpec(string number)
    {
        Query.Where(a => a.AccrualNumber == number);
    }
}

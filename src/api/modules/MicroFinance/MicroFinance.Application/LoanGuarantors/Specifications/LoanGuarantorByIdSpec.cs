using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Specifications;

public sealed class LoanGuarantorByIdSpec : Specification<LoanGuarantor>, ISingleResultSpecification<LoanGuarantor>
{
    public LoanGuarantorByIdSpec(Guid id)
    {
        Query.Where(lg => lg.Id == id);
    }
}

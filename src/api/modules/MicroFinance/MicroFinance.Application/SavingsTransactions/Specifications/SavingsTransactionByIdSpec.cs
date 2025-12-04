using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Specifications;

public sealed class SavingsTransactionByIdSpec : Specification<SavingsTransaction>, ISingleResultSpecification<SavingsTransaction>
{
    public SavingsTransactionByIdSpec(Guid id)
    {
        Query.Where(t => t.Id == id);
    }
}

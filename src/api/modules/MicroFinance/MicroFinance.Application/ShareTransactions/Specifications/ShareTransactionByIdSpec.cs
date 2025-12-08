using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Specifications;

public sealed class ShareTransactionByIdSpec : Specification<ShareTransaction>, ISingleResultSpecification<ShareTransaction>
{
    public ShareTransactionByIdSpec(DefaultIdType id)
    {
        Query.Where(t => t.Id == id);
    }
}

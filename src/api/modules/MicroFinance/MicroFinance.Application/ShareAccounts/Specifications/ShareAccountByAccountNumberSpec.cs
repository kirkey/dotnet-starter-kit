using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Specifications;

public sealed class ShareAccountByAccountNumberSpec : Specification<ShareAccount>, ISingleResultSpecification<ShareAccount>
{
    public ShareAccountByAccountNumberSpec(string accountNumber)
    {
        Query.Where(sa => sa.AccountNumber == accountNumber);
    }
}

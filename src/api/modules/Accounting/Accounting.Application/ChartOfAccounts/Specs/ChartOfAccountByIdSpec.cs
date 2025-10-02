using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Application.ChartOfAccounts.Queries;

public sealed class ChartOfAccountByIdSpec :
    Specification<ChartOfAccount, ChartOfAccountResponse>,
    ISingleResultSpecification<ChartOfAccount, ChartOfAccountResponse>
{
    public ChartOfAccountByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}

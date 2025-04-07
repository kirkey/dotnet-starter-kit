using Accounting.Application.Accounts.Dtos;
using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using Accounting.Domain;

namespace Accounting.Application.Accounts.Search.v1;
public sealed class AccountSearchSpec : EntitiesByPaginationFilterSpec<Account, AccountDto>
{
    public AccountSearchSpec(AccountSearchRequest request)
        : base(request)
    {
        Query
            .OrderBy(c => c.Code, !request.HasOrderBy())
            .Where(a => a.Code.Contains(request.Keyword!)
                || a.Name.Contains(request.Keyword!)
                || a.Description!.Contains(request.Keyword!)
                || a.Notes!.Contains(request.Keyword!),
                !string.IsNullOrEmpty(request.Keyword));
    }
}

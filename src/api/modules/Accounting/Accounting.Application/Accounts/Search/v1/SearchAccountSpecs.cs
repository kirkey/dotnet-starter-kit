using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using Accounting.Application.Accounts.Get.v1;
using Accounting.Domain;

namespace Accounting.Application.Accounts.Search.v1;
public sealed class SearchAccountSpecs : EntitiesByPaginationFilterSpec<Account, AccountResponse>
{
    public SearchAccountSpecs(SearchAccountsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Code, !command.HasOrderBy())
            .Where(a => a.Code.Contains(command.Keyword!), !string.IsNullOrEmpty(command.Keyword))
            .Where(a => a.Name.Contains(command.Keyword!), !string.IsNullOrEmpty(command.Keyword))
            .Where(a => a.Description!.Contains(command.Keyword!), !string.IsNullOrEmpty(command.Keyword))
            .Where(a => a.Notes!.Contains(command.Keyword!), !string.IsNullOrEmpty(command.Keyword));
}

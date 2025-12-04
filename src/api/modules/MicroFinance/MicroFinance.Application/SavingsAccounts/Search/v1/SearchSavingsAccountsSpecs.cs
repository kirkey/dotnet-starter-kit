using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Search.v1;

/// <summary>
/// Specification for searching savings accounts.
/// </summary>
public sealed class SearchSavingsAccountsSpecs : EntitiesByPaginationFilterSpec<SavingsAccount, SavingsAccountResponse>
{
    public SearchSavingsAccountsSpecs(SearchSavingsAccountsCommand command)
        : base(command) =>
        Query
            .Include(x => x.Member)
            .Include(x => x.SavingsProduct)
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.SavingsProductId == command.SavingsProductId!.Value, command.SavingsProductId.HasValue)
            .Where(x => x.Status == command.Status, !string.IsNullOrEmpty(command.Status))
            .Where(x => x.AccountNumber.Contains(command.Keyword!) || 
                       (x.Member != null && x.Member.FullName.Contains(command.Keyword!)), 
                   !string.IsNullOrEmpty(command.Keyword));
}

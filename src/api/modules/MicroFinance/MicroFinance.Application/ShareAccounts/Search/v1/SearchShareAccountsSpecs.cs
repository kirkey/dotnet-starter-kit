using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Search.v1;

public class SearchShareAccountsSpecs : EntitiesByPaginationFilterSpec<ShareAccount, ShareAccountResponse>
{
    public SearchShareAccountsSpecs(SearchShareAccountsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(sa => sa.OpenedDate, !command.HasOrderBy())
            .Where(sa => sa.AccountNumber.Contains(command.AccountNumber!), !string.IsNullOrWhiteSpace(command.AccountNumber))
            .Where(sa => sa.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(sa => sa.ShareProductId == command.ShareProductId!.Value, command.ShareProductId.HasValue)
            .Where(sa => sa.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(sa => sa.OpenedDate >= command.OpenedDateFrom!.Value, command.OpenedDateFrom.HasValue)
            .Where(sa => sa.OpenedDate <= command.OpenedDateTo!.Value, command.OpenedDateTo.HasValue);
}

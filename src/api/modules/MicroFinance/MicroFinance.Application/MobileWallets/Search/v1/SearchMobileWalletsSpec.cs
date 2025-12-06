using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Search.v1;

public class SearchMobileWalletsSpec : EntitiesByPaginationFilterSpec<MobileWallet, MobileWalletResponse>
{
    public SearchMobileWalletsSpec(SearchMobileWalletsCommand command)
        : base(command) =>
        Query
            .OrderBy(w => w.PhoneNumber, !command.HasOrderBy())
            .Where(w => w.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(w => w.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(w => w.Tier == command.Tier, !string.IsNullOrWhiteSpace(command.Tier))
            .Where(w => w.Provider.Contains(command.Provider!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.Provider))
            .Where(w => w.PhoneNumber.Contains(command.PhoneNumber!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.PhoneNumber))
            .Where(w => w.IsLinkedToBankAccount == command.IsLinkedToBankAccount!.Value, command.IsLinkedToBankAccount.HasValue);
}

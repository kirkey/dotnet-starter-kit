using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Search.v1;

public class SearchCashVaultsSpecs : EntitiesByPaginationFilterSpec<CashVault, CashVaultSummaryResponse>
{
    public SearchCashVaultsSpecs(SearchCashVaultsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.BranchId == command.BranchId!.Value, command.BranchId.HasValue)
            .Where(x => x.Code.Contains(command.Code!), !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.VaultType == command.VaultType, !string.IsNullOrWhiteSpace(command.VaultType))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.CustodianUserId == command.CustodianUserId!.Value, command.CustodianUserId.HasValue)
            .Where(x => x.CurrentBalance >= command.MinCurrentBalance!.Value, command.MinCurrentBalance.HasValue)
            .Where(x => x.CurrentBalance <= command.MaxCurrentBalance!.Value, command.MaxCurrentBalance.HasValue);
}

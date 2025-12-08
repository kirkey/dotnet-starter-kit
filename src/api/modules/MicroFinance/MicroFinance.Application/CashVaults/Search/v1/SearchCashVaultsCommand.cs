using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Search.v1;

public class SearchCashVaultsCommand : PaginationFilter, IRequest<PagedList<CashVaultSummaryResponse>>
{
    public DefaultIdType? BranchId { get; set; }
    public string? Code { get; set; }
    public string? VaultType { get; set; }
    public string? Status { get; set; }
    public DefaultIdType? CustodianUserId { get; set; }
    public decimal? MinCurrentBalance { get; set; }
    public decimal? MaxCurrentBalance { get; set; }
}

public sealed record CashVaultSummaryResponse(
    DefaultIdType Id,
    DefaultIdType BranchId,
    string Code,
    string VaultType,
    decimal CurrentBalance,
    decimal OpeningBalance,
    decimal MinimumBalance,
    decimal MaximumBalance,
    string? Location,
    string? CustodianName,
    DefaultIdType? CustodianUserId,
    DateTime? LastReconciliationDate,
    decimal? LastReconciledBalance,
    string Status
);

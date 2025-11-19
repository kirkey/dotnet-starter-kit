using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Search.v1;

/// <summary>
/// Request to search leave balances with filtering and pagination.
/// </summary>
public class SearchLeaveBalancesRequest : PaginationFilter, IRequest<PagedList<LeaveBalanceResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public DefaultIdType? LeaveTypeId { get; set; }
    public int? Year { get; set; }
}


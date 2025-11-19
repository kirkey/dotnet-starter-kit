using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Search.v1;

public sealed class SearchLeaveBalancesHandler(
    [FromKeyedServices("hr:leavebalances")] IReadRepository<LeaveBalance> repository)
    : IRequestHandler<SearchLeaveBalancesRequest, PagedList<LeaveBalanceResponse>>
{
    public async Task<PagedList<LeaveBalanceResponse>> Handle(
        SearchLeaveBalancesRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchLeaveBalancesSpec(request);
        var balances = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = balances.Select(b => new LeaveBalanceResponse
        {
            Id = b.Id,
            EmployeeId = b.EmployeeId,
            LeaveTypeId = b.LeaveTypeId,
            Year = b.Year,
            OpeningBalance = b.OpeningBalance,
            AccruedDays = b.AccruedDays,
            CarriedOverDays = b.CarriedOverDays,
            AvailableDays = b.AvailableDays,
            TakenDays = b.TakenDays,
            PendingDays = b.PendingDays,
            RemainingDays = b.RemainingDays,
            CarryoverExpiryDate = b.CarryoverExpiryDate
        }).ToList();

        return new PagedList<LeaveBalanceResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}


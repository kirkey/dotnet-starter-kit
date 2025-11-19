using FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Get.v1;

/// <summary>
/// Handler for retrieving a leave balance by ID.
/// </summary>
public sealed class GetLeaveBalanceHandler(
    [FromKeyedServices("hr:leavebalances")] IReadRepository<LeaveBalance> repository)
    : IRequestHandler<GetLeaveBalanceRequest, LeaveBalanceResponse>
{
    public async Task<LeaveBalanceResponse> Handle(
        GetLeaveBalanceRequest request,
        CancellationToken cancellationToken)
    {
        var balance = await repository
            .FirstOrDefaultAsync(new LeaveBalanceByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (balance is null)
            throw new Exception($"Leave balance not found: {request.Id}");

        return new LeaveBalanceResponse
        {
            Id = balance.Id,
            EmployeeId = balance.EmployeeId,
            LeaveTypeId = balance.LeaveTypeId,
            Year = balance.Year,
            OpeningBalance = balance.OpeningBalance,
            AccruedDays = balance.AccruedDays,
            CarriedOverDays = balance.CarriedOverDays,
            AvailableDays = balance.AvailableDays,
            TakenDays = balance.TakenDays,
            PendingDays = balance.PendingDays,
            RemainingDays = balance.RemainingDays,
            CarryoverExpiryDate = balance.CarryoverExpiryDate
        };
    }
}


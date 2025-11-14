namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Cancel.v1;

/// <summary>
/// Handler for canceling leave request with balance restoration.
/// Releases pending balance if request was submitted but not approved.
/// </summary>
public sealed class CancelLeaveRequestHandler(
    ILogger<CancelLeaveRequestHandler> logger,
    [FromKeyedServices("hr:leaverequests")] IRepository<LeaveRequest> repository,
    [FromKeyedServices("hr:leavebalances")] IRepository<LeaveBalance> leaveBalanceRepository)
    : IRequestHandler<CancelLeaveRequestCommand, CancelLeaveRequestResponse>
{
    public async Task<CancelLeaveRequestResponse> Handle(
        CancelLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Fetch leave request
        var leaveRequest = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (leaveRequest is null)
            throw new LeaveRequestNotFoundException(request.Id);

        var previousStatus = leaveRequest.Status;

        // Cancel request
        leaveRequest.Cancel(request.Reason);

        // If request was submitted (not yet approved), release pending balance
        if (previousStatus == "Submitted")
        {
            var balanceSpec = new LeaveBalances.Specifications.LeaveBalanceByEmployeeAndYearSpec(
                leaveRequest.EmployeeId,
                leaveRequest.LeaveTypeId,
                leaveRequest.StartDate.Year);

            var balance = await leaveBalanceRepository.FirstOrDefaultAsync(balanceSpec, cancellationToken);

            if (balance is not null)
            {
                // Remove pending (restore balance)
                balance.RemovePending(leaveRequest.NumberOfDays);
                await leaveBalanceRepository.UpdateAsync(balance, cancellationToken);

                logger.LogInformation(
                    "Leave request {RequestId} cancelled by Employee {EmployeeId}. Balance restored. Remaining: {Remaining}",
                    leaveRequest.Id,
                    leaveRequest.EmployeeId,
                    balance.RemainingDays);
            }
        }

        await repository.UpdateAsync(leaveRequest, cancellationToken);

        logger.LogInformation(
            "Leave request {RequestId} cancelled by Employee {EmployeeId}. Previous Status: {PreviousStatus}",
            leaveRequest.Id,
            leaveRequest.EmployeeId,
            previousStatus);

        return new CancelLeaveRequestResponse(leaveRequest.Id, leaveRequest.Status);
    }
}


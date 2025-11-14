namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Reject.v1;

/// <summary>
/// Handler for rejecting leave request with balance restoration.
/// Releases pending balance back to available.
/// </summary>
public sealed class RejectLeaveRequestHandler(
    ILogger<RejectLeaveRequestHandler> logger,
    [FromKeyedServices("hr:leaverequests")] IRepository<LeaveRequest> repository,
    [FromKeyedServices("hr:leavebalances")] IRepository<LeaveBalance> leaveBalanceRepository)
    : IRequestHandler<RejectLeaveRequestCommand, RejectLeaveRequestResponse>
{
    public async Task<RejectLeaveRequestResponse> Handle(
        RejectLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Fetch leave request
        var leaveRequest = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (leaveRequest is null)
            throw new LeaveRequestNotFoundException(request.Id);

        // Reject request
        leaveRequest.Reject(request.Reason);

        // Update balance: Release pending
        var balanceSpec = new LeaveBalances.Specifications.LeaveBalanceByEmployeeAndYearSpec(
            leaveRequest.EmployeeId,
            leaveRequest.LeaveTypeId,
            leaveRequest.StartDate.Year);

        var balance = await leaveBalanceRepository.FirstOrDefaultAsync(balanceSpec, cancellationToken);

        if (balance is null)
        {
            throw new InvalidOperationException(
                $"Leave balance not found for Employee {leaveRequest.EmployeeId}, Year {leaveRequest.StartDate.Year}");
        }

        // Remove pending (restore balance)
        balance.RemovePending(leaveRequest.NumberOfDays);

        await leaveBalanceRepository.UpdateAsync(balance, cancellationToken);
        await repository.UpdateAsync(leaveRequest, cancellationToken);

        logger.LogInformation(
            "Leave request {RequestId} rejected for Employee {EmployeeId}. Days: {Days}, Balance Restored. Remaining: {Remaining}",
            leaveRequest.Id,
            leaveRequest.EmployeeId,
            leaveRequest.NumberOfDays,
            balance.RemainingDays);

        return new RejectLeaveRequestResponse(
            leaveRequest.Id,
            leaveRequest.Status,
            leaveRequest.ReviewedDate!.Value,
            request.Reason);
    }
}


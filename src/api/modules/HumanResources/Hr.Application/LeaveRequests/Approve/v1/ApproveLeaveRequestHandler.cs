namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Approve.v1;

/// <summary>
/// Handler for approving leave request with balance update.
/// Converts pending balance to taken (approved leave).
/// </summary>
public sealed class ApproveLeaveRequestHandler(
    ILogger<ApproveLeaveRequestHandler> logger,
    [FromKeyedServices("hr:leaverequests")] IRepository<LeaveRequest> repository,
    [FromKeyedServices("hr:leavebalances")] IRepository<LeaveBalance> leaveBalanceRepository)
    : IRequestHandler<ApproveLeaveRequestCommand, ApproveLeaveRequestResponse>
{
    public async Task<ApproveLeaveRequestResponse> Handle(
        ApproveLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Fetch leave request
        var leaveRequest = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (leaveRequest is null)
            throw new LeaveRequestNotFoundException(request.Id);

        // Approve request
        leaveRequest.Approve(request.Comment);

        // Update balance: Convert pending to taken
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

        // Convert pending to taken
        balance.ApprovePending(leaveRequest.NumberOfDays);

        await leaveBalanceRepository.UpdateAsync(balance, cancellationToken);
        await repository.UpdateAsync(leaveRequest, cancellationToken);

        logger.LogInformation(
            "Leave request {RequestId} approved for Employee {EmployeeId}. Days: {Days}, Balance Taken: {Taken}, Remaining: {Remaining}",
            leaveRequest.Id,
            leaveRequest.EmployeeId,
            leaveRequest.NumberOfDays,
            balance.TakenDays,
            balance.RemainingDays);

        return new ApproveLeaveRequestResponse(
            leaveRequest.Id,
            leaveRequest.Status,
            leaveRequest.ReviewedDate!.Value);
    }
}


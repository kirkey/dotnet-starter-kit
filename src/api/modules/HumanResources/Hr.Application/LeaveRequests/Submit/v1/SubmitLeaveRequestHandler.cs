namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Submit.v1;

/// <summary>
/// Handler for submitting leave request with Philippines Labor Code compliance.
/// Validates leave balance, eligibility, and reserves balance.
/// </summary>
public sealed class SubmitLeaveRequestHandler(
    ILogger<SubmitLeaveRequestHandler> logger,
    [FromKeyedServices("hr:leaverequests")] IRepository<LeaveRequest> repository,
    [FromKeyedServices("hr:leavebalances")] IRepository<LeaveBalance> leaveBalanceRepository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:leavetypes")] IReadRepository<LeaveType> leaveTypeRepository)
    : IRequestHandler<SubmitLeaveRequestCommand, SubmitLeaveRequestResponse>
{
    public async Task<SubmitLeaveRequestResponse> Handle(
        SubmitLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Fetch leave request
        var leaveRequest = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (leaveRequest is null)
            throw new LeaveRequestNotFoundException(request.Id);

        // Fetch employee
        var employee = await employeeRepository.GetByIdAsync(leaveRequest.EmployeeId, cancellationToken);

        if (employee is null)
            throw new EmployeeNotFoundException(leaveRequest.EmployeeId);

        // Fetch leave type
        var leaveType = await leaveTypeRepository.GetByIdAsync(leaveRequest.LeaveTypeId, cancellationToken);

        if (leaveType is null)
            throw new LeaveTypeNotFoundException(leaveRequest.LeaveTypeId);

        // Philippines-Specific: Check eligibility per Labor Code
        var (isEligible, reason) = leaveType.CheckEligibility(
            employee.Gender,
            employee.HireDate,
            leaveRequest.StartDate);

        if (!isEligible)
            throw new InvalidOperationException($"Employee not eligible for this leave: {reason}");

        // Find or create leave balance
        var balanceSpec = new LeaveBalances.Specifications.LeaveBalanceByEmployeeAndYearSpec(
            leaveRequest.EmployeeId,
            leaveRequest.LeaveTypeId,
            leaveRequest.StartDate.Year);

        var balance = await leaveBalanceRepository.FirstOrDefaultAsync(balanceSpec, cancellationToken);

        if (balance is null)
        {
            // Create balance if it doesn't exist
            balance = LeaveBalance.Create(
                leaveRequest.EmployeeId,
                leaveRequest.LeaveTypeId,
                leaveRequest.StartDate.Year,
                openingBalance: 0);

            await leaveBalanceRepository.AddAsync(balance, cancellationToken);
        }

        // Check sufficient balance
        if (leaveRequest.NumberOfDays > balance.RemainingDays)
        {
            throw new InvalidOperationException(
                $"Insufficient leave balance. Available: {balance.RemainingDays} days, Requested: {leaveRequest.NumberOfDays} days.");
        }

        // Reserve balance (add to pending)
        balance.AddPending(leaveRequest.NumberOfDays);
        await leaveBalanceRepository.UpdateAsync(balance, cancellationToken);

        // Submit request
        leaveRequest.Submit(request.ApproverManagerId);
        await repository.UpdateAsync(leaveRequest, cancellationToken);

        logger.LogInformation(
            "Leave request {RequestId} submitted by Employee {EmployeeId}. Days: {Days}, Balance Reserved: {Pending}",
            leaveRequest.Id,
            leaveRequest.EmployeeId,
            leaveRequest.NumberOfDays,
            balance.PendingDays);

        return new SubmitLeaveRequestResponse(
            leaveRequest.Id,
            leaveRequest.Status,
            leaveRequest.SubmittedDate!.Value);
    }
}


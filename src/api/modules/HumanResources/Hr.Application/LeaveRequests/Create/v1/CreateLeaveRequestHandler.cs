namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Create.v1;

public sealed class CreateLeaveRequestHandler(
    ILogger<CreateLeaveRequestHandler> logger,
    [FromKeyedServices("hr:leaverequests")] IRepository<LeaveRequest> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:leavetypes")] IReadRepository<LeaveType> leaveTypeRepository)
    : IRequestHandler<CreateLeaveRequestCommand, CreateLeaveRequestResponse>
{
    public async Task<CreateLeaveRequestResponse> Handle(
        CreateLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        var leaveType = await leaveTypeRepository
            .GetByIdAsync(request.LeaveTypeId, cancellationToken)
            .ConfigureAwait(false);

        if (leaveType is null)
            throw new LeaveTypeNotFoundException(request.LeaveTypeId);

        var leaveRequest = LeaveRequest.Create(
            request.EmployeeId,
            request.LeaveTypeId,
            request.StartDate,
            request.EndDate,
            request.Reason);

        var approverId = request.ApproverManagerId ?? employee.Id;
        leaveRequest.Submit(approverId);

        await repository.AddAsync(leaveRequest, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Leave request created with ID {LeaveRequestId} for Employee {EmployeeId}, {StartDate}-{EndDate}",
            leaveRequest.Id,
            request.EmployeeId,
            request.StartDate,
            request.EndDate);

        return new CreateLeaveRequestResponse(leaveRequest.Id);
    }
}


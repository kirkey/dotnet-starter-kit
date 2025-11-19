namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Create.v1;

public sealed class CreateLeaveBalanceHandler(
    ILogger<CreateLeaveBalanceHandler> logger,
    [FromKeyedServices("hr:leavebalances")] IRepository<LeaveBalance> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:leavetypes")] IReadRepository<LeaveType> leaveTypeRepository)
    : IRequestHandler<CreateLeaveBalanceCommand, CreateLeaveBalanceResponse>
{
    public async Task<CreateLeaveBalanceResponse> Handle(
        CreateLeaveBalanceCommand request,
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

        var balance = LeaveBalance.Create(
            request.EmployeeId,
            request.LeaveTypeId,
            request.Year,
            request.OpeningBalance);

        await repository.AddAsync(balance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Leave balance created with ID {LeaveBalanceId} for Employee {EmployeeId}, Year {Year}",
            balance.Id,
            request.EmployeeId,
            request.Year);

        return new CreateLeaveBalanceResponse(balance.Id);
    }
}


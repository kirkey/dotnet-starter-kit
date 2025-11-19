namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Accrue.v1;

/// <summary>
/// Handler for accruing leave days per Philippines Labor Code accrual rules.
/// Creates balance if it doesn't exist for the year.
/// </summary>
public sealed class AccrueLeaveHandler(
    ILogger<AccrueLeaveHandler> logger,
    [FromKeyedServices("hr:leavebalances")] IRepository<LeaveBalance> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:leavetypes")] IReadRepository<LeaveType> leaveTypeRepository)
    : IRequestHandler<AccrueLeaveCommand, AccrueLeaveResponse>
{
    public async Task<AccrueLeaveResponse> Handle(
        AccrueLeaveCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate employee exists
        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        // Validate leave type exists
        var leaveType = await leaveTypeRepository
            .GetByIdAsync(request.LeaveTypeId, cancellationToken)
            .ConfigureAwait(false);

        if (leaveType is null)
            throw new LeaveTypeNotFoundException(request.LeaveTypeId);

        // Find or create balance for the year
        var spec = new Specifications.LeaveBalanceByEmployeeAndYearSpec(request.EmployeeId, request.LeaveTypeId, request.Year);
        var balance = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (balance is null)
        {
            // Create new balance for the year
            balance = LeaveBalance.Create(
                request.EmployeeId,
                request.LeaveTypeId,
                request.Year,
                openingBalance: 0);

            await repository.AddAsync(balance, cancellationToken).ConfigureAwait(false);

            logger.LogInformation(
                "Created new leave balance for Employee {EmployeeId}, LeaveType {LeaveTypeId}, Year {Year}",
                request.EmployeeId,
                request.LeaveTypeId,
                request.Year);
        }

        // Add accrual
        balance.AddAccrual(request.DaysToAccrue);

        await repository.UpdateAsync(balance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Accrued {Days} days for Employee {EmployeeId}, LeaveType {LeaveTypeId}, Year {Year}. Total Accrued: {TotalAccrued}, Remaining: {Remaining}",
            request.DaysToAccrue,
            request.EmployeeId,
            request.LeaveTypeId,
            request.Year,
            balance.AccruedDays,
            balance.RemainingDays);

        return new AccrueLeaveResponse(balance.Id, balance.AccruedDays, balance.RemainingDays);
    }
}


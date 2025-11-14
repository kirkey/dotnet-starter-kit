namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Create.v1;

public sealed class CreateTimesheetHandler(
    ILogger<CreateTimesheetHandler> logger,
    [FromKeyedServices("hr:timesheets")] IRepository<Timesheet> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreateTimesheetCommand, CreateTimesheetResponse>
{
    public async Task<CreateTimesheetResponse> Handle(
        CreateTimesheetCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        var timesheet = Timesheet.Create(
            request.EmployeeId,
            request.StartDate,
            request.EndDate,
            request.PeriodType);

        await repository.AddAsync(timesheet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Timesheet created with ID {TimesheetId} for Employee {EmployeeId}, Period {StartDate}-{EndDate}",
            timesheet.Id,
            employee.Id,
            request.StartDate,
            request.EndDate);

        return new CreateTimesheetResponse(timesheet.Id);
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Create.v1;

public sealed class CreateTimesheetHandler(
    ILogger<CreateTimesheetHandler> logger,
    [FromKeyedServices("hr:timesheets")] IRepository<Domain.Entities.Timesheet> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Domain.Entities.Employee> employeeRepository)
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

        var timesheet = Domain.Entities.Timesheet.Create(
            request.EmployeeId,
            request.StartDate,
            request.EndDate,
            request.PeriodType);

        await repository.AddAsync(timesheet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Timesheet created with ID {TimesheetId}, Employee {EmployeeId}, Period {StartDate}-{EndDate}",
            timesheet.Id,
            timesheet.EmployeeId,
            timesheet.StartDate.Date,
            timesheet.EndDate.Date);

        return new CreateTimesheetResponse(timesheet.Id);
    }
}


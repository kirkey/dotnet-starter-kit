namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Create.v1;

public sealed class CreateAttendanceHandler(
    ILogger<CreateAttendanceHandler> logger,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:attendance")] IRepository<Domain.Entities.Attendance> repository)
    : IRequestHandler<CreateAttendanceCommand, CreateAttendanceResponse>
{
    public async Task<CreateAttendanceResponse> Handle(
        CreateAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        var attendance = Domain.Entities.Attendance.Create(
            request.EmployeeId,
            request.AttendanceDate,
            request.ClockInTime,
            request.ClockOutTime,
            request.ClockInLocation,
            request.ClockOutLocation);

        await repository.AddAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance created with ID {AttendanceId}, Employee {EmployeeId}, Date {AttendanceDate}",
            attendance.Id,
            attendance.EmployeeId,
            attendance.AttendanceDate.ToString("MMM d, yyyy"));

        return new CreateAttendanceResponse(attendance.Id);
    }
}


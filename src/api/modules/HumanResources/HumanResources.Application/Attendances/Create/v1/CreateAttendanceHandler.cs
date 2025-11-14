namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Create.v1;

/// <summary>
/// Handler for creating an attendance record (clock in).
/// </summary>
public sealed class CreateAttendanceHandler(
    ILogger<CreateAttendanceHandler> logger,
    [FromKeyedServices("hr:attendance")] IRepository<Attendance> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreateAttendanceCommand, CreateAttendanceResponse>
{
    /// <summary>
    /// Handles the request to create an attendance record.
    /// </summary>
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

        var attendance = Attendance.Create(
            request.EmployeeId,
            DateTime.Today,
            request.ClockInTime,
            null,
            request.ClockInLocation,
            null);

        await repository.AddAsync(attendance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Attendance record created with ID {AttendanceId} for Employee {EmployeeId} at {ClockInTime}",
            attendance.Id,
            employee.Id,
            request.ClockInTime);

        return new CreateAttendanceResponse(attendance.Id);
    }
}


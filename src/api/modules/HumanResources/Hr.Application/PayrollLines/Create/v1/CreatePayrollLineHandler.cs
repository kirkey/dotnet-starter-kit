namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Create.v1;

/// <summary>
/// Handler for creating a payroll line.
/// </summary>
public sealed class CreatePayrollLineHandler(
    ILogger<CreatePayrollLineHandler> logger,
    [FromKeyedServices("hr:payrolllines")] IRepository<PayrollLine> repository,
    [FromKeyedServices("hr:payrolls")] IReadRepository<Payroll> payrollRepository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreatePayrollLineCommand, CreatePayrollLineResponse>
{
    public async Task<CreatePayrollLineResponse> Handle(
        CreatePayrollLineCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payroll = await payrollRepository
            .GetByIdAsync(request.PayrollId, cancellationToken)
            .ConfigureAwait(false);

        if (payroll is null)
            throw new Exception($"Payroll not found: {request.PayrollId}");

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new Exception($"Employee not found: {request.EmployeeId}");

        var line = PayrollLine.Create(
            request.PayrollId,
            request.EmployeeId,
            request.RegularHours,
            request.OvertimeHours);

        await repository.AddAsync(line, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Payroll line created with ID {PayrollLineId} for Employee {EmployeeId} in Payroll {PayrollId}",
            line.Id,
            request.EmployeeId,
            request.PayrollId);

        return new CreatePayrollLineResponse(line.Id);
    }
}


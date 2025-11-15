namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Process.v1;

/// <summary>
/// Handler for processing a payroll period.
/// Initiates pay calculations and transitions to Processing status.
/// </summary>
public sealed class ProcessPayrollHandler(
    ILogger<ProcessPayrollHandler> logger,
    [FromKeyedServices("hr:payrolls")] IRepository<Payroll> repository)
    : IRequestHandler<ProcessPayrollCommand, ProcessPayrollResponse>
{
    public async Task<ProcessPayrollResponse> Handle(
        ProcessPayrollCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payroll = await repository.GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (payroll is null)
            throw new PayrollNotFoundException(request.Id);

        payroll.Process();

        await repository.UpdateAsync(payroll, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation(
            "Payroll {PayrollId} processed successfully. Period: {StartDate}-{EndDate}, Employee Count: {EmployeeCount}",
            payroll.Id,
            payroll.StartDate,
            payroll.EndDate,
            payroll.EmployeeCount);

        return new ProcessPayrollResponse(
            payroll.Id,
            payroll.Status,
            payroll.ProcessedDate!.Value);
    }
}


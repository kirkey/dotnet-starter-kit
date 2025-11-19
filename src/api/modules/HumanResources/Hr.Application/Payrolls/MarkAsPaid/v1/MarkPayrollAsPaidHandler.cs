namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.MarkAsPaid.v1;

/// <summary>
/// Handler for marking payroll as paid.
/// </summary>
public sealed class MarkPayrollAsPaidHandler(
    ILogger<MarkPayrollAsPaidHandler> logger,
    [FromKeyedServices("hr:payrolls")] IRepository<Payroll> repository)
    : IRequestHandler<MarkPayrollAsPaidCommand, MarkPayrollAsPaidResponse>
{
    public async Task<MarkPayrollAsPaidResponse> Handle(
        MarkPayrollAsPaidCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payroll = await repository.GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (payroll is null)
            throw new PayrollNotFoundException(request.Id);

        payroll.MarkAsPaid();

        await repository.UpdateAsync(payroll, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation(
            "Payroll {PayrollId} marked as paid. Status: {Status}, Paid Date: {PaidDate}",
            payroll.Id,
            payroll.Status,
            payroll.PaidDate);

        return new MarkPayrollAsPaidResponse(
            payroll.Id,
            payroll.Status,
            payroll.PaidDate!.Value);
    }
}


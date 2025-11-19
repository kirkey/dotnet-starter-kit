namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.CompleteProcessing.v1;

/// <summary>
/// Handler for completing payroll processing.
/// </summary>
public sealed class CompletePayrollProcessingHandler(
    ILogger<CompletePayrollProcessingHandler> logger,
    [FromKeyedServices("hr:payrolls")] IRepository<Payroll> repository)
    : IRequestHandler<CompletePayrollProcessingCommand, CompletePayrollProcessingResponse>
{
    public async Task<CompletePayrollProcessingResponse> Handle(
        CompletePayrollProcessingCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payroll = await repository.GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (payroll is null)
            throw new PayrollNotFoundException(request.Id);

        payroll.CompleteProcessing();

        await repository.UpdateAsync(payroll, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation(
            "Payroll {PayrollId} processing completed. Status: {Status}",
            payroll.Id,
            payroll.Status);

        return new CompletePayrollProcessingResponse(payroll.Id, payroll.Status);
    }
}


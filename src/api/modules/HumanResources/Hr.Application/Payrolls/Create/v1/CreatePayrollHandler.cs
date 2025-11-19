namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Create.v1;

/// <summary>
/// Handler for creating a payroll period.
/// </summary>
public sealed class CreatePayrollHandler(
    ILogger<CreatePayrollHandler> logger,
    [FromKeyedServices("hr:payrolls")] IRepository<Payroll> repository)
    : IRequestHandler<CreatePayrollCommand, CreatePayrollResponse>
{
    public async Task<CreatePayrollResponse> Handle(
        CreatePayrollCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payroll = Payroll.Create(
            request.StartDate,
            request.EndDate,
            request.PayFrequency);

        await repository.AddAsync(payroll, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Payroll created with ID {PayrollId}, Period {StartDate}-{EndDate}",
            payroll.Id,
            request.StartDate,
            request.EndDate);

        return new CreatePayrollResponse(payroll.Id);
    }
}


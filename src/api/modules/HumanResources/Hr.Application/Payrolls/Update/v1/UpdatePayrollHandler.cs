namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Update.v1;

/// <summary>
/// Handler for updating a payroll record.
/// </summary>
public sealed class UpdatePayrollHandler(
    ILogger<UpdatePayrollHandler> logger,
    [FromKeyedServices("hr:payrolls")] IRepository<Payroll> repository)
    : IRequestHandler<UpdatePayrollCommand, UpdatePayrollResponse>
{
    public async Task<UpdatePayrollResponse> Handle(
        UpdatePayrollCommand request,
        CancellationToken cancellationToken)
    {
        var payroll = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (payroll is null)
            throw new NotFoundException($"Payroll not found: {request.Id}");

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status.ToLower())
            {
                case "processing":
                    payroll.Process();
                    break;
                case "processed":
                    payroll.CompleteProcessing();
                    break;
                case "posted":
                    if (string.IsNullOrWhiteSpace(request.JournalEntryId))
                        throw new ArgumentException("Journal entry ID is required for posting", nameof(request.JournalEntryId));
                    payroll.Post(request.JournalEntryId);
                    break;
                case "paid":
                    payroll.MarkAsPaid();
                    break;
            }
        }

        await repository.UpdateAsync(payroll, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Payroll {PayrollId} updated to status {Status}", payroll.Id, payroll.Status);

        return new UpdatePayrollResponse(payroll.Id);
    }
}


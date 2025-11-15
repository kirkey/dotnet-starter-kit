namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Post.v1;

/// <summary>
/// Handler for posting payroll to the general ledger.
/// </summary>
public sealed class PostPayrollHandler(
    ILogger<PostPayrollHandler> logger,
    [FromKeyedServices("hr:payrolls")] IRepository<Payroll> repository)
    : IRequestHandler<PostPayrollCommand, PostPayrollResponse>
{
    public async Task<PostPayrollResponse> Handle(
        PostPayrollCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payroll = await repository.GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (payroll is null)
            throw new PayrollNotFoundException(request.Id);

        payroll.Post(request.JournalEntryId);

        await repository.UpdateAsync(payroll, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation(
            "Payroll {PayrollId} posted to GL with Journal Entry {JournalEntryId}. Status: {Status}",
            payroll.Id,
            request.JournalEntryId,
            payroll.Status);

        return new PostPayrollResponse(
            payroll.Id,
            payroll.Status,
            payroll.PostedDate!.Value,
            payroll.JournalEntryId!);
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Delete.v1;

/// <summary>
/// Handler for deleting a payroll record.
/// </summary>
public sealed class DeletePayrollHandler(
    ILogger<DeletePayrollHandler> logger,
    [FromKeyedServices("hr:payrolls")] IRepository<Payroll> repository)
    : IRequestHandler<DeletePayrollCommand, DeletePayrollResponse>
{
    public async Task<DeletePayrollResponse> Handle(
        DeletePayrollCommand request,
        CancellationToken cancellationToken)
    {
        var payroll = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (payroll is null)
            throw new Exception($"Payroll not found: {request.Id}");

        if (payroll.IsLocked)
            throw new InvalidOperationException("Cannot delete locked payroll");

        await repository.DeleteAsync(payroll, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Payroll {PayrollId} deleted successfully", payroll.Id);

        return new DeletePayrollResponse(payroll.Id);
    }
}

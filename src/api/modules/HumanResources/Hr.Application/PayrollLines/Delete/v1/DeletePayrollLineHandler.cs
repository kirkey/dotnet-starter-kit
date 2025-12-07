namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Delete.v1;

/// <summary>
/// Handler for deleting a payroll line.
/// </summary>
public sealed class DeletePayrollLineHandler(
    ILogger<DeletePayrollLineHandler> logger,
    [FromKeyedServices("hr:payrolllines")] IRepository<PayrollLine> repository)
    : IRequestHandler<DeletePayrollLineCommand, DeletePayrollLineResponse>
{
    public async Task<DeletePayrollLineResponse> Handle(
        DeletePayrollLineCommand request,
        CancellationToken cancellationToken)
    {
        var line = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (line is null)
            throw new NotFoundException($"Payroll line not found: {request.Id}");

        await repository.DeleteAsync(line, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Payroll line {PayrollLineId} deleted successfully", line.Id);

        return new DeletePayrollLineResponse(line.Id);
    }
}

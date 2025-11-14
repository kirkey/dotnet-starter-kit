namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Delete.v1;

public sealed class DeletePayrollDeductionHandler(
    ILogger<DeletePayrollDeductionHandler> logger,
    [FromKeyedServices("humanresources:payrolldeductions")] IRepository<PayrollDeduction> repository)
    : IRequestHandler<DeletePayrollDeductionCommand, DeletePayrollDeductionResponse>
{
    public async Task<DeletePayrollDeductionResponse> Handle(DeletePayrollDeductionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = deduction ?? throw new PayrollDeductionNotFoundException(request.Id);

        await repository.DeleteAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Payroll deduction with id {DeductionId} deleted.", deduction.Id);

        return new DeletePayrollDeductionResponse(deduction.Id);
    }
}


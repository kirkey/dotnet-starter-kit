namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Delete.v1;

/// <summary>
/// Handler for deleting payroll deduction.
/// </summary>
public sealed class DeletePayrollDeductionHandler(
    ILogger<DeletePayrollDeductionHandler> logger,
    [FromKeyedServices("hr:payrolldeductions")] IRepository<PayrollDeduction> repository)
    : IRequestHandler<DeletePayrollDeductionCommand, DeletePayrollDeductionResponse>
{
    public async Task<DeletePayrollDeductionResponse> Handle(
        DeletePayrollDeductionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (deduction is null)
            throw new PayrollDeductionNotFoundException(request.Id);

        await repository.DeleteAsync(deduction, cancellationToken);

        logger.LogInformation(
            "Payroll deduction {Id} deleted",
            deduction.Id);

        return new DeletePayrollDeductionResponse(deduction.Id, true);
    }
}


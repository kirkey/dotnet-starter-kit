namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Update.v1;

public sealed class UpdatePayrollDeductionHandler(
    ILogger<UpdatePayrollDeductionHandler> logger,
    [FromKeyedServices("humanresources:payrolldeductions")] IRepository<PayrollDeduction> repository)
    : IRequestHandler<UpdatePayrollDeductionCommand, UpdatePayrollDeductionResponse>
{
    public async Task<UpdatePayrollDeductionResponse> Handle(UpdatePayrollDeductionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = deduction ?? throw new PayrollDeductionNotFoundException(request.Id);

        // Update deduction amounts
        if (request.DeductionAmount.HasValue)
        {
            deduction.UpdateDeductionAmount(request.DeductionAmount.Value);
        }

        if (request.DeductionPercentage.HasValue)
        {
            deduction.UpdateDeductionPercentage(request.DeductionPercentage.Value);
        }

        // Update authorization and recoverability
        if (request.IsAuthorized.HasValue)
        {
            deduction.SetAsAuthorized(request.IsAuthorized.Value);
        }

        if (request.IsRecoverable.HasValue)
        {
            deduction.SetRecoverable(request.IsRecoverable.Value);
        }

        // Update end date
        if (request.EndDate.HasValue)
        {
            deduction.SetDateRange(deduction.StartDate, request.EndDate.Value);
        }

        // Update max deduction limit
        if (request.MaxDeductionLimit.HasValue)
        {
            deduction.SetMaxDeductionLimit(request.MaxDeductionLimit.Value);
        }

        await repository.UpdateAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Payroll deduction with id {DeductionId} updated.", deduction.Id);

        return new UpdatePayrollDeductionResponse(deduction.Id);
    }
}


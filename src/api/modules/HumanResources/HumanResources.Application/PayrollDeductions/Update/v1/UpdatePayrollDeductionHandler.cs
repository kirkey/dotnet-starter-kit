namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Update.v1;

/// <summary>
/// Handler for updating payroll deduction.
/// </summary>
public sealed class UpdatePayrollDeductionHandler(
    ILogger<UpdatePayrollDeductionHandler> logger,
    [FromKeyedServices("hr:payrolldeductions")] IRepository<PayrollDeduction> repository)
    : IRequestHandler<UpdatePayrollDeductionCommand, UpdatePayrollDeductionResponse>
{
    public async Task<UpdatePayrollDeductionResponse> Handle(
        UpdatePayrollDeductionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (deduction is null)
            throw new PayrollDeductionNotFoundException(request.Id);

        // Update deduction amount if provided
        if (request.DeductionAmount.HasValue)
            deduction.UpdateDeductionAmount(request.DeductionAmount.Value);

        // Update deduction percentage if provided
        if (request.DeductionPercentage.HasValue)
            deduction.UpdateDeductionPercentage(request.DeductionPercentage.Value);

        // Update end date if provided
        if (request.EndDate.HasValue)
            deduction.UpdateEndDate(request.EndDate.Value);
        else if (request.EndDate == null && request.EndDate != null)
            deduction.UpdateEndDate(null);

        // Update max deduction limit if provided
        if (request.MaxDeductionLimit.HasValue)
            deduction.SetMaxDeductionLimit(request.MaxDeductionLimit.Value);

        // Update remarks if provided
        if (!string.IsNullOrWhiteSpace(request.Remarks))
            deduction.UpdateRemarks(request.Remarks);

        // Update active status if provided
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                deduction.Activate();
            else
                deduction.Deactivate();
        }

        await repository.UpdateAsync(deduction, cancellationToken);

        logger.LogInformation(
            "Payroll deduction {Id} updated: Amount {Amount}, Active {Active}",
            deduction.Id,
            request.DeductionAmount,
            request.IsActive);

        return new UpdatePayrollDeductionResponse(
            deduction.Id,
            deduction.DeductionType,
            request.DeductionAmount ?? deduction.DeductionAmount,
            deduction.IsActive);
    }
}


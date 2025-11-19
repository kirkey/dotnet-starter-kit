namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Update.v1;

/// <summary>
/// Handler for updating a payroll line.
/// </summary>
public sealed class UpdatePayrollLineHandler(
    ILogger<UpdatePayrollLineHandler> logger,
    [FromKeyedServices("hr:payrolllines")] IRepository<PayrollLine> repository)
    : IRequestHandler<UpdatePayrollLineCommand, UpdatePayrollLineResponse>
{
    public async Task<UpdatePayrollLineResponse> Handle(
        UpdatePayrollLineCommand request,
        CancellationToken cancellationToken)
    {
        var line = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (line is null)
            throw new Exception($"Payroll line not found: {request.Id}");

        if (request.RegularHours.HasValue || request.OvertimeHours.HasValue)
            line.SetHours(
                request.RegularHours ?? line.RegularHours,
                request.OvertimeHours ?? line.OvertimeHours);

        if (request.RegularPay.HasValue || request.OvertimePay.HasValue || 
            request.BonusPay.HasValue || request.OtherEarnings.HasValue)
            line.SetEarnings(
                request.RegularPay ?? line.RegularPay,
                request.OvertimePay ?? line.OvertimePay,
                request.BonusPay ?? line.BonusPay,
                request.OtherEarnings ?? line.OtherEarnings);

        if (request.IncomeTax.HasValue || request.SocialSecurityTax.HasValue ||
            request.MedicareTax.HasValue || request.OtherTaxes.HasValue)
            line.SetTaxes(
                request.IncomeTax ?? line.IncomeTax,
                request.SocialSecurityTax ?? line.SocialSecurityTax,
                request.MedicareTax ?? line.MedicareTax,
                request.OtherTaxes ?? line.OtherTaxes);

        if (request.HealthInsurance.HasValue || request.RetirementContribution.HasValue || request.OtherDeductions.HasValue)
            line.SetDeductions(
                request.HealthInsurance ?? line.HealthInsurance,
                request.RetirementContribution ?? line.RetirementContribution,
                request.OtherDeductions ?? line.OtherDeductions);

        if (!string.IsNullOrWhiteSpace(request.PaymentMethod))
            line.SetPaymentMethod(
                request.PaymentMethod,
                request.BankAccountLast4 ?? line.BankAccountLast4,
                request.CheckNumber ?? line.CheckNumber);

        line.RecalculateTotals();

        await repository.UpdateAsync(line, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Payroll line {PayrollLineId} updated successfully with new calculations", line.Id);

        return new UpdatePayrollLineResponse(line.Id);
    }
}


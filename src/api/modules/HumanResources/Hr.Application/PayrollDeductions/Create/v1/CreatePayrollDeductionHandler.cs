namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Create.v1;

public sealed class CreatePayrollDeductionHandler(
    ILogger<CreatePayrollDeductionHandler> logger,
    [FromKeyedServices("humanresources:payrolldeductions")] IRepository<PayrollDeduction> repository)
    : IRequestHandler<CreatePayrollDeductionCommand, CreatePayrollDeductionResponse>
{
    public async Task<CreatePayrollDeductionResponse> Handle(CreatePayrollDeductionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = PayrollDeduction.Create(
            request.PayComponentId,
            request.DeductionType,
            request.DeductionAmount,
            request.DeductionPercentage);

        // Set employee or organizational unit
        if (request.EmployeeId.HasValue)
        {
            deduction.SetEmployee(request.EmployeeId.Value);
        }
        else if (request.OrganizationalUnitId.HasValue)
        {
            deduction.SetOrganizationalUnit(request.OrganizationalUnitId.Value);
        }

        // Set authorization and recoverability
        deduction.SetAsAuthorized(request.IsAuthorized);
        deduction.SetRecoverable(request.IsRecoverable);

        // Set date range
        if (request.StartDate.HasValue)
        {
            deduction.SetDateRange(request.StartDate.Value, request.EndDate);
        }

        // Set max deduction limit
        if (request.MaxDeductionLimit.HasValue)
        {
            deduction.SetMaxDeductionLimit(request.MaxDeductionLimit.Value);
        }

        // Set reference number
        if (!string.IsNullOrWhiteSpace(request.ReferenceNumber))
        {
            deduction.SetReferenceNumber(request.ReferenceNumber);
        }

        await repository.AddAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Payroll deduction created {DeductionId} for component {PayComponentId}, type {DeductionType}",
            deduction.Id,
            request.PayComponentId,
            request.DeductionType);

        return new CreatePayrollDeductionResponse(deduction.Id);
    }
}


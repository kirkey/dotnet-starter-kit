namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Create.v1;

public sealed class CreateEmployeePayComponentHandler(
    ILogger<CreateEmployeePayComponentHandler> logger,
    [FromKeyedServices("hr:employeepaycomponents")] IRepository<EmployeePayComponent> repository)
    : IRequestHandler<CreateEmployeePayComponentCommand, CreateEmployeePayComponentResponse>
{
    public async Task<CreateEmployeePayComponentResponse> Handle(CreateEmployeePayComponentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        EmployeePayComponent assignment;

        // Determine assignment type and create appropriately
        if (request.IsOneTime && request.OneTimeDate.HasValue && request.FixedAmount.HasValue)
        {
            // One-time payment/deduction
            assignment = EmployeePayComponent.CreateOneTime(
                request.EmployeeId,
                request.PayComponentId,
                request.FixedAmount.Value,
                request.OneTimeDate.Value,
                request.ReferenceNumber);
        }
        else if (request.InstallmentCount.HasValue && request.TotalAmount.HasValue)
        {
            // Loan/installment deduction
            assignment = EmployeePayComponent.CreateLoan(
                request.EmployeeId,
                request.PayComponentId,
                request.TotalAmount.Value,
                request.InstallmentCount.Value,
                request.EffectiveStartDate ?? DateTime.UtcNow,
                request.ReferenceNumber);
        }
        else if (request.CustomRate.HasValue && request.AssignmentType == "Override")
        {
            // Rate override
            assignment = EmployeePayComponent.CreateRateOverride(
                request.EmployeeId,
                request.PayComponentId,
                request.CustomRate.Value,
                request.EffectiveStartDate ?? DateTime.UtcNow,
                request.EffectiveEndDate);
        }
        else if (request.FixedAmount.HasValue)
        {
            // Fixed amount addition
            assignment = EmployeePayComponent.CreateFixedAmount(
                request.EmployeeId,
                request.PayComponentId,
                request.FixedAmount.Value,
                request.EffectiveStartDate ?? DateTime.UtcNow,
                request.EffectiveEndDate,
                request.ReferenceNumber);
        }
        else
        {
            throw new ArgumentException("Invalid assignment configuration. Must specify either FixedAmount, CustomRate, or Loan details.");
        }

        // Update additional properties
        if (!string.IsNullOrWhiteSpace(request.CustomFormula) || !string.IsNullOrWhiteSpace(request.Remarks))
        {
            assignment.Update(
                customFormula: request.CustomFormula,
                remarks: request.Remarks);
        }

        await repository.AddAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee pay component created {AssignmentId} for employee {EmployeeId}, component {PayComponentId}",
            assignment.Id,
            request.EmployeeId,
            request.PayComponentId);

        return new CreateEmployeePayComponentResponse(assignment.Id);
    }
}


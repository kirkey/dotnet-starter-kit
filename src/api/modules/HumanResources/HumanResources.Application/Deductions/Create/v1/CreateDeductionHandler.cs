namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;

/// <summary>
/// Handler for creating a deduction type.
/// </summary>
public sealed class CreateDeductionHandler(
    ILogger<CreateDeductionHandler> logger,
    [FromKeyedServices("hr:deductions")] IRepository<Deduction> repository)
    : IRequestHandler<CreateDeductionCommand, CreateDeductionResponse>
{
    public async Task<CreateDeductionResponse> Handle(
        CreateDeductionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = Deduction.Create(
            request.DeductionName,
            request.DeductionType,
            request.RecoveryMethod);

        // Set recovery details if provided
        deduction.SetRecoveryDetails(
            request.RecoveryFixedAmount,
            request.RecoveryPercentage,
            request.InstallmentCount);

        // Set compliance rules
        deduction.SetMaxRecoveryPercentage(request.MaxRecoveryPercentage);
        deduction.SetRequiresApproval(request.RequiresApproval);
        deduction.SetIsRecurring(request.IsRecurring);

        // Set GL account code and description
        deduction.Update(
            glAccountCode: request.GlAccountCode,
            description: request.Description);

        await repository.AddAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Deduction created: ID {Id}, Name {Name}, Type {Type}, Recovery {Method}",
            deduction.Id,
            deduction.DeductionName,
            deduction.DeductionType,
            deduction.RecoveryMethod);

        return new CreateDeductionResponse(
            deduction.Id,
            deduction.DeductionName,
            deduction.DeductionType,
            deduction.IsActive);
    }
}


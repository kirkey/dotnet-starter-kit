namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;

/// <summary>
/// Handler for updating a deduction.
/// </summary>
public sealed class UpdateDeductionHandler(
    ILogger<UpdateDeductionHandler> logger,
    [FromKeyedServices("hr:deductions")] IRepository<Deduction> repository)
    : IRequestHandler<UpdateDeductionCommand, UpdateDeductionResponse>
{
    public async Task<UpdateDeductionResponse> Handle(
        UpdateDeductionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (deduction is null)
            throw new DeductionNotFoundException(request.Id);

        // Update basic properties
        deduction.Update(
            request.DeductionName,
            request.DeductionType,
            request.RecoveryMethod,
            request.GlAccountCode,
            request.Description);

        // Update recovery details
        if (request.RecoveryFixedAmount.HasValue || request.RecoveryPercentage.HasValue || request.InstallmentCount.HasValue)
        {
            deduction.SetRecoveryDetails(
                request.RecoveryFixedAmount,
                request.RecoveryPercentage,
                request.InstallmentCount);
        }

        // Update compliance rules
        if (request.MaxRecoveryPercentage.HasValue)
            deduction.SetMaxRecoveryPercentage(request.MaxRecoveryPercentage.Value);

        if (request.RequiresApproval.HasValue)
            deduction.SetRequiresApproval(request.RequiresApproval.Value);

        if (request.IsRecurring.HasValue)
            deduction.SetIsRecurring(request.IsRecurring.Value);

        // Update active status
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                deduction.Activate();
            else
                deduction.Deactivate();
        }

        await repository.UpdateAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Deduction {Id} updated: Name {Name}, Active {Active}",
            deduction.Id, deduction.DeductionName, deduction.IsActive);

        return new UpdateDeductionResponse(
            deduction.Id,
            deduction.DeductionName,
            deduction.IsActive);
    }
}


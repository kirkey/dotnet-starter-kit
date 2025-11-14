namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;

/// <summary>
/// Handler for updating a deduction.
/// </summary>
public sealed class UpdateDeductionHandler(
    ILogger<UpdateDeductionHandler> logger,
    [FromKeyedServices("hr:deductions")] IRepository<PayComponent> repository)
    : IRequestHandler<UpdateDeductionCommand, UpdateDeductionResponse>
{
    public async Task<UpdateDeductionResponse> Handle(
        UpdateDeductionCommand request,
        CancellationToken cancellationToken)
    {
        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (deduction is null)
            throw new Exception($"Deduction not found: {request.Id}");

        deduction.Update(
            componentName: request.ComponentName,
            glAccountCode: request.GLAccountCode,
            description: request.Description);

        await repository.UpdateAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Deduction {DeductionId} updated successfully", deduction.Id);

        return new UpdateDeductionResponse(deduction.Id);
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;

/// <summary>
/// Handler for creating a deduction.
/// </summary>
public sealed class CreateDeductionHandler(
    ILogger<CreateDeductionHandler> logger,
    [FromKeyedServices("hr:deductions")] IRepository<PayComponent> repository)
    : IRequestHandler<CreateDeductionCommand, CreateDeductionResponse>
{
    public async Task<CreateDeductionResponse> Handle(
        CreateDeductionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = PayComponent.Create(
            request.ComponentName,
            request.ComponentType,
            request.GlAccountCode);

        if (!string.IsNullOrWhiteSpace(request.Description))
            deduction.Update(description: request.Description);

        await repository.AddAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Deduction created with ID {DeductionId}, Name {ComponentName}, Type {ComponentType}",
            deduction.Id,
            request.ComponentName,
            request.ComponentType);

        return new CreateDeductionResponse(deduction.Id);
    }
}


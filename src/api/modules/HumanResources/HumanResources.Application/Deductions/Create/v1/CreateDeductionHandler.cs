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

        // Generate code from component name if not provided
        var code = request.ComponentName.Replace(" ", "_", StringComparison.Ordinal).ToUpperInvariant();

        var deduction = PayComponent.Create(
            code,
            request.ComponentName,
            request.ComponentType,
            calculationMethod: "Manual", // Default to Manual for deductions
            request.GlAccountCode);

        if (!string.IsNullOrWhiteSpace(request.Description))
            deduction.Update(description: request.Description);

        await repository.AddAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Deduction created with ID {DeductionId}, Code {Code}, Name {ComponentName}, Type {ComponentType}",
            deduction.Id,
            code,
            request.ComponentName,
            request.ComponentType);

        return new CreateDeductionResponse(deduction.Id);
    }
}


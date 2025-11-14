namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Handler for updating benefit.
/// </summary>
public sealed class UpdateBenefitHandler(
    ILogger<UpdateBenefitHandler> logger,
    [FromKeyedServices("hr:benefits")] IRepository<Benefit> repository)
    : IRequestHandler<UpdateBenefitCommand, UpdateBenefitResponse>
{
    public async Task<UpdateBenefitResponse> Handle(
        UpdateBenefitCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var benefit = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (benefit is null)
            throw new BenefitNotFoundException(request.Id);

        // Update contributions if provided
        if (request.EmployeeContribution.HasValue || request.EmployerContribution.HasValue)
        {
            benefit.UpdateContributions(
                request.EmployeeContribution,
                request.EmployerContribution);
        }

        // Update coverage details if provided
        if (!string.IsNullOrWhiteSpace(request.CoverageType) || 
            !string.IsNullOrWhiteSpace(request.ProviderName) || 
            request.CoverageAmount.HasValue)
        {
            benefit.SetCoverageDetails(
                request.CoverageType,
                request.CoverageAmount,
                request.ProviderName);
        }

        // Update description if provided
        if (!string.IsNullOrWhiteSpace(request.Description))
            benefit.SetDescription(request.Description);

        // Update active status if provided
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                benefit.Activate();
            else
                benefit.Deactivate();
        }

        await repository.UpdateAsync(benefit, cancellationToken);

        logger.LogInformation(
            "Benefit {Id} updated: Active {Active}",
            benefit.Id,
            benefit.IsActive);

        return new UpdateBenefitResponse(
            benefit.Id,
            benefit.BenefitName,
            benefit.IsActive);
    }
}


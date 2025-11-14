namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;

/// <summary>
/// Handler for creating benefit.
/// </summary>
public sealed class CreateBenefitHandler(
    ILogger<CreateBenefitHandler> logger,
    [FromKeyedServices("hr:benefits")] IRepository<Benefit> repository)
    : IRequestHandler<CreateBenefitCommand, CreateBenefitResponse>
{
    public async Task<CreateBenefitResponse> Handle(
        CreateBenefitCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var benefit = Benefit.Create(
            request.BenefitName,
            request.BenefitType,
            request.EmployeeContribution,
            request.EmployerContribution);

        // Set as mandatory if specified
        if (request.IsMandatory)
            benefit.SetAsMandatory(true);

        // Set effective dates (start today)
        benefit.SetEffectiveDates(DateTime.UtcNow);

        // Set coverage details if provided
        if (!string.IsNullOrWhiteSpace(request.CoverageType) || 
            !string.IsNullOrWhiteSpace(request.ProviderName) || 
            request.CoverageAmount.HasValue)
        {
            benefit.SetCoverageDetails(
                request.CoverageType,
                request.CoverageAmount,
                request.ProviderName);
        }

        // Set waiting period if provided
        if (request.WaitingPeriodDays.HasValue)
            benefit.SetWaitingPeriod(request.WaitingPeriodDays.Value);

        // Set description if provided
        if (!string.IsNullOrWhiteSpace(request.Description))
            benefit.SetDescription(request.Description);

        await repository.AddAsync(benefit, cancellationToken);

        logger.LogInformation(
            "Benefit created: ID {Id}, Name {Name}, Type {Type}, Mandatory {Mandatory}",
            benefit.Id,
            benefit.BenefitName,
            benefit.BenefitType,
            benefit.IsMandatory);

        return new CreateBenefitResponse(
            benefit.Id,
            benefit.BenefitName,
            benefit.BenefitType,
            benefit.IsMandatory,
            benefit.IsActive);
    }
}


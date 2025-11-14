namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;

/// <summary>
/// Handler for creating a benefit.
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

        if (!string.IsNullOrWhiteSpace(request.Description) || request.AnnualLimit.HasValue || request.MinimumEligibleEmployees.HasValue)
        {
            benefit.Update(description: request.Description);
        }

        await repository.AddAsync(benefit, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Benefit created with ID {BenefitId}, Name {BenefitName}, Type {BenefitType}",
            benefit.Id,
            request.BenefitName,
            request.BenefitType);

        return new CreateBenefitResponse(benefit.Id);
    }
}


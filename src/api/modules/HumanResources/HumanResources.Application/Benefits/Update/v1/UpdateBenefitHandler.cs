namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Handler for updating a benefit.
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
        var benefit = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (benefit is null)
            throw new Exception($"Benefit not found: {request.Id}");

        benefit.Update(
            benefitName: request.BenefitName,
            employeeContribution: request.EmployeeContribution,
            employerContribution: request.EmployerContribution,
            description: request.Description);

        if (request.IsRequired.HasValue)
        {
            if (request.IsRequired.Value)
                benefit.MakeRequired();
            else
                benefit.MakeOptional();
        }

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                benefit.Activate();
            else
                benefit.Deactivate();
        }

        await repository.UpdateAsync(benefit, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Benefit {BenefitId} updated successfully", benefit.Id);

        return new UpdateBenefitResponse(benefit.Id);
    }
}


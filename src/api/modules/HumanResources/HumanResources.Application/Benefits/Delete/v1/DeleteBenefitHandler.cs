namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Delete.v1;

/// <summary>
/// Handler for deleting benefit.
/// </summary>
public sealed class DeleteBenefitHandler(
    ILogger<DeleteBenefitHandler> logger,
    [FromKeyedServices("hr:benefits")] IRepository<Benefit> repository)
    : IRequestHandler<DeleteBenefitCommand, DeleteBenefitResponse>
{
    public async Task<DeleteBenefitResponse> Handle(
        DeleteBenefitCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var benefit = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (benefit is null)
            throw new BenefitNotFoundException(request.Id);

        await repository.DeleteAsync(benefit, cancellationToken);

        logger.LogInformation("Benefit {Id} deleted", benefit.Id);

        return new DeleteBenefitResponse(benefit.Id, true);
    }
}


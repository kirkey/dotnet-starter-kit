namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Delete.v1;

/// <summary>
/// Handler for deleting designation.
/// </summary>
public sealed class DeleteDesignationHandler(
    ILogger<DeleteDesignationHandler> logger,
    [FromKeyedServices("hr:designations")] IRepository<Designation> repository)
    : IRequestHandler<DeleteDesignationCommand, DeleteDesignationResponse>
{
    public async Task<DeleteDesignationResponse> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
    {
        var designation = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (designation is null)
        {
            throw new DesignationNotFoundException(request.Id);
        }

        await repository.DeleteAsync(designation, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Designation {DesignationId} deleted successfully", designation.Id);

        return new DeleteDesignationResponse(designation.Id);
    }
}


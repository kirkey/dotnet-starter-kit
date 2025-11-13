namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Update.v1;

/// <summary>
/// Handler for updating designation.
/// </summary>
public sealed class UpdateDesignationHandler(
    ILogger<UpdateDesignationHandler> logger,
    [FromKeyedServices("hr:designations")] IRepository<Designation> repository)
    : IRequestHandler<UpdateDesignationCommand, UpdateDesignationResponse>
{
    public async Task<UpdateDesignationResponse> Handle(UpdateDesignationCommand request, CancellationToken cancellationToken)
    {
        var designation = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (designation is null)
        {
            throw new DesignationNotFoundException(request.Id);
        }

        designation.Update(request.Title, request.Description, request.MinSalary, request.MaxSalary);

        await repository.UpdateAsync(designation, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Designation {DesignationId} updated successfully", designation.Id);

        return new UpdateDesignationResponse(designation.Id);
    }
}


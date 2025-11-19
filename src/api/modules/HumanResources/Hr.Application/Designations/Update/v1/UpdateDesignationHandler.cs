namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Update.v1;

/// <summary>
/// Handler for updating a designation with area-specific salary configuration.
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

        designation.Update(
            request.Title,
            request.Area,
            request.Description,
            request.SalaryGrade,
            request.MinimumSalary,
            request.MaximumSalary,
            request.IsManagerial,
            request.IsActive);

        await repository.UpdateAsync(designation, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Designation {DesignationId} updated: Title={Title}, Area={Area}, Grade={SalaryGrade}, Active={IsActive}",
            designation.Id,
            designation.Title,
            designation.Area,
            designation.SalaryGrade,
            designation.IsActive);

        return new UpdateDesignationResponse(designation.Id);
    }
}


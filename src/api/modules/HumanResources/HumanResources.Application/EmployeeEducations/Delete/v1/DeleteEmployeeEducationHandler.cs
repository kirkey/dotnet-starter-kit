namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Delete.v1;

/// <summary>
/// Handler for deleting an employee education record.
/// </summary>
public sealed class DeleteEmployeeEducationHandler(
    ILogger<DeleteEmployeeEducationHandler> logger,
    [FromKeyedServices("hr:employeeeducations")] IRepository<FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation> repository)
    : IRequestHandler<DeleteEmployeeEducationCommand, DeleteEmployeeEducationResponse>
{
    public async Task<DeleteEmployeeEducationResponse> Handle(
        DeleteEmployeeEducationCommand request,
        CancellationToken cancellationToken)
    {
        var education = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (education is null)
            throw new Exception($"Employee education record not found: {request.Id}");

        await repository.DeleteAsync(education, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee education record {EducationId} deleted successfully", education.Id);

        return new DeleteEmployeeEducationResponse(education.Id);
    }
}


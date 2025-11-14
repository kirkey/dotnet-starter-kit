namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Update.v1;

/// <summary>
/// Handler for updating an employee education record.
/// </summary>
public sealed class UpdateEmployeeEducationHandler(
    ILogger<UpdateEmployeeEducationHandler> logger,
    [FromKeyedServices("hr:employeeeducations")] IRepository<FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation> repository)
    : IRequestHandler<UpdateEmployeeEducationCommand, UpdateEmployeeEducationResponse>
{
    public async Task<UpdateEmployeeEducationResponse> Handle(
        UpdateEmployeeEducationCommand request,
        CancellationToken cancellationToken)
    {
        var education = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (education is null)
            throw new Exception($"Employee education record not found: {request.Id}");

        education.Update(
            fieldOfStudy: request.FieldOfStudy,
            degree: request.Degree,
            gpa: request.Gpa,
            notes: request.Notes);

        if (request.MarkAsVerified)
            education.MarkAsVerified();

        await repository.UpdateAsync(education, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee education record {EducationId} updated successfully", education.Id);

        return new UpdateEmployeeEducationResponse(education.Id);
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Create.v1;

/// <summary>
/// Handler for creating an employee education record.
/// </summary>
public sealed class CreateEmployeeEducationHandler(
    ILogger<CreateEmployeeEducationHandler> logger,
    [FromKeyedServices("hr:employeeeducations")] IRepository<FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreateEmployeeEducationCommand, CreateEmployeeEducationResponse>
{
    public async Task<CreateEmployeeEducationResponse> Handle(
        CreateEmployeeEducationCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new Exception($"Employee not found: {request.EmployeeId}");

        var education = FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation.Create(
            request.EmployeeId,
            request.EducationLevel,
            request.FieldOfStudy,
            request.Institution,
            request.GraduationDate,
            request.Degree,
            request.Gpa,
            request.CertificateNumber,
            request.CertificationDate);

        await repository.AddAsync(education, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee education record created with ID {EducationId}, Employee {EmployeeId}, Level {EducationLevel}",
            education.Id,
            request.EmployeeId,
            request.EducationLevel);

        return new CreateEmployeeEducationResponse(education.Id);
    }
}


using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;

/// <summary>
/// Handler for retrieving an employee education record by ID.
/// </summary>
public sealed class GetEmployeeEducationHandler(
    [FromKeyedServices("hr:employeeeducations")] IReadRepository<FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation> repository)
    : IRequestHandler<GetEmployeeEducationRequest, EmployeeEducationResponse>
{
    public async Task<EmployeeEducationResponse> Handle(
        GetEmployeeEducationRequest request,
        CancellationToken cancellationToken)
    {
        var education = await repository
            .FirstOrDefaultAsync(new EmployeeEducationByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (education is null)
            throw new Exception($"Employee education record not found: {request.Id}");

        return MapToResponse(education);
    }

    private static EmployeeEducationResponse MapToResponse(FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation education)
    {
        return new EmployeeEducationResponse
        {
            Id = education.Id,
            EmployeeId = education.EmployeeId,
            EducationLevel = education.EducationLevel,
            FieldOfStudy = education.FieldOfStudy,
            Institution = education.Institution,
            GraduationDate = education.GraduationDate,
            Degree = education.Degree,
            Gpa = education.Gpa,
            CertificateNumber = education.CertificateNumber,
            CertificationDate = education.CertificationDate,
            IsActive = education.IsActive,
            IsVerified = education.IsVerified,
            VerificationDate = education.VerificationDate,
            Notes = education.Notes
        };
    }
}


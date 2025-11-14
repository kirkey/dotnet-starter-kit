using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Search.v1;

/// <summary>
/// Handler for searching employee education records.
/// </summary>
public sealed class SearchEmployeeEducationsHandler(
    [FromKeyedServices("hr:employeeeducations")] IReadRepository<EmployeeEducation> repository)
    : IRequestHandler<SearchEmployeeEducationsRequest, PagedList<EmployeeEducationResponse>>
{
    public async Task<PagedList<EmployeeEducationResponse>> Handle(
        SearchEmployeeEducationsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchEmployeeEducationsSpec(request);
        var educations = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = educations.Select(MapToResponse).ToList();

        return new PagedList<EmployeeEducationResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }

    private static EmployeeEducationResponse MapToResponse(EmployeeEducation education)
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


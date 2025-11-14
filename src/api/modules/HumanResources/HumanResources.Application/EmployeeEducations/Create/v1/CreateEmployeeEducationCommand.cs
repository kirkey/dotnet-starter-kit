namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Create.v1;

/// <summary>
/// Command to create a new employee education record.
/// </summary>
public sealed record CreateEmployeeEducationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("Bachelor")] string EducationLevel,
    [property: DefaultValue("Computer Science")] string FieldOfStudy,
    [property: DefaultValue("State University")] string Institution,
    [property: DefaultValue("2020-05-15")] DateTime GraduationDate,
    [property: DefaultValue(null)] string? Degree = null,
    [property: DefaultValue(null)] decimal? Gpa = null,
    [property: DefaultValue(null)] string? CertificateNumber = null,
    [property: DefaultValue(null)] DateTime? CertificationDate = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<CreateEmployeeEducationResponse>;


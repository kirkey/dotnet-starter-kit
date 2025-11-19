namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Update.v1;

/// <summary>
/// Command to update an employee education record.
/// </summary>
public sealed record UpdateEmployeeEducationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? FieldOfStudy = null,
    [property: DefaultValue(null)] string? Degree = null,
    [property: DefaultValue(null)] decimal? Gpa = null,
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue(false)] bool MarkAsVerified = false) : IRequest<UpdateEmployeeEducationResponse>;


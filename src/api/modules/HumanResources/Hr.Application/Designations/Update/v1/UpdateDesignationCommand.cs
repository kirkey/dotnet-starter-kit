namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Update.v1;

/// <summary>
/// Command to update an existing designation with area-specific salary configuration.
/// </summary>
public sealed record UpdateDesignationCommand(
    DefaultIdType Id,
    [property: DefaultValue("Senior Software Engineer")] string Title,
    [property: DefaultValue("Metro Manila")] string? Area = "National",
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue("Grade 3")] string? SalaryGrade = "Grade 1",
    [property: DefaultValue(null)] decimal? MinimumSalary = null,
    [property: DefaultValue(null)] decimal? MaximumSalary = null,
    [property: DefaultValue(false)] bool IsManagerial = false,
    [property: DefaultValue(true)] bool IsActive = true) : IRequest<UpdateDesignationResponse>;


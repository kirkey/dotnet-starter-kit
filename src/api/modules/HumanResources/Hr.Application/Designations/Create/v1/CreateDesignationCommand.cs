namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Create.v1;

/// <summary>
/// Command to create a new designation with area-specific salary configuration.
/// </summary>
public sealed record CreateDesignationCommand(
    [property: DefaultValue("ENG-001")] string Code,
    [property: DefaultValue("Senior Software Engineer")] string Title,
    [property: DefaultValue("Metro Manila")] string? Area = "National",
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue("Grade 3")] string? SalaryGrade = "Grade 1",
    [property: DefaultValue(null)] decimal? MinimumSalary = null,
    [property: DefaultValue(null)] decimal? MaximumSalary = null,
    [property: DefaultValue(false)] bool IsManagerial = false) : IRequest<CreateDesignationResponse>;


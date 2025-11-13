namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Update.v1;

/// <summary>
/// Command to update designation.
/// </summary>
public sealed record UpdateDesignationCommand(
    DefaultIdType Id,
    [property: DefaultValue("Supervisor")] string Title,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] decimal? MinSalary = null,
    [property: DefaultValue(null)] decimal? MaxSalary = null) : IRequest<UpdateDesignationResponse>;


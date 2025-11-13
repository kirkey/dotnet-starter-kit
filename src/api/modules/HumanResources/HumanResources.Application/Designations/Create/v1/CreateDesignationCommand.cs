namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Create.v1;

/// <summary>
/// Command to create a new designation.
/// </summary>
public sealed record CreateDesignationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType OrganizationalUnitId,
    [property: DefaultValue("SUP-001")] string Code,
    [property: DefaultValue("Supervisor")] string Title,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] decimal? MinSalary = null,
    [property: DefaultValue(null)] decimal? MaxSalary = null) : IRequest<CreateDesignationResponse>;


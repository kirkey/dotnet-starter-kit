namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Update.v1;

/// <summary>
/// Command to update company.
/// </summary>
public sealed record UpdateCompanyCommand(
    DefaultIdType Id,
    [property: DefaultValue("Sample Company Inc.")] string Name,
    [property: DefaultValue(null)] string? TIN = null,
    [property: DefaultValue(null)] string? Address = null,
    [property: DefaultValue(null)] string? ZipCode = null,
    [property: DefaultValue(null)] string? Phone = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? Website = null) : IRequest<UpdateCompanyResponse>;


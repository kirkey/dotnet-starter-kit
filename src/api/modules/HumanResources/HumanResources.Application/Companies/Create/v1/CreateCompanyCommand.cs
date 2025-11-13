using System.ComponentModel;

namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Create.v1;

/// <summary>
/// Command to create a new company.
/// </summary>
public sealed record CreateCompanyCommand(
    [property: DefaultValue("COMP001")] string CompanyCode,
    [property: DefaultValue("Sample Company Inc.")] string Name,
    [property: DefaultValue(null)] string? Tin = null) : IRequest<CreateCompanyResponse>;


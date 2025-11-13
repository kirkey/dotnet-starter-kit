using System.ComponentModel;

namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Create.v1;

/// <summary>
/// Command to create a new company.
/// </summary>
public sealed record CreateCompanyCommand(
    [property: DefaultValue("COMP001")] string CompanyCode,
    [property: DefaultValue("Sample Company Inc.")] string LegalName,
    [property: DefaultValue("Sample Company")] string? TradeName = null,
    [property: DefaultValue(null)] string? TaxId = null,
    [property: DefaultValue("USD")] string BaseCurrency = "USD",
    [property: DefaultValue(12)] int FiscalYearEnd = 12,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<CreateCompanyResponse>;


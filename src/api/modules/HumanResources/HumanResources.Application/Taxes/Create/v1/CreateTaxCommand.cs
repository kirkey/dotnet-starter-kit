namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Command to create a new tax master configuration.
/// </summary>
public sealed record CreateTaxCommand(
    [property: DefaultValue("VAT-STD")] string Code,
    [property: DefaultValue("Standard VAT")] string Name,
    [property: DefaultValue("VAT")] string TaxType,
    [property: DefaultValue(0.10)] decimal Rate,
    [property: DefaultValue(null)] DefaultIdType? TaxCollectedAccountId = null,
    [property: DefaultValue(null)] DateTime? EffectiveDate = null,
    [property: DefaultValue(false)] bool IsCompound = false,
    [property: DefaultValue(null)] string? Jurisdiction = null,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null,
    [property: DefaultValue(null)] DefaultIdType? TaxPaidAccountId = null,
    [property: DefaultValue(null)] string? TaxAuthority = null,
    [property: DefaultValue(null)] string? TaxRegistrationNumber = null,
    [property: DefaultValue(null)] string? ReportingCategory = null) : IRequest<CreateTaxResponse>;


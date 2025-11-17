namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Command to update an existing tax master configuration.
/// </summary>
public sealed record UpdateTaxCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? Name = null,
    [property: DefaultValue(null)] string? TaxType = null,
    [property: DefaultValue(null)] decimal? Rate = null,
    [property: DefaultValue(null)] bool? IsCompound = null,
    [property: DefaultValue(null)] string? Jurisdiction = null,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null,
    [property: DefaultValue(null)] DefaultIdType? TaxPaidAccountId = null,
    [property: DefaultValue(null)] string? TaxAuthority = null,
    [property: DefaultValue(null)] string? TaxRegistrationNumber = null,
    [property: DefaultValue(null)] string? ReportingCategory = null) : IRequest<DefaultIdType>;


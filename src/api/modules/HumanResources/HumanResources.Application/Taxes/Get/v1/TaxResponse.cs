namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

/// <summary>
/// Response object for tax retrieval.
/// Contains complete tax master configuration details.
/// </summary>
public sealed record TaxResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string TaxType,
    decimal Rate,
    bool IsCompound,
    string? Jurisdiction,
    DateTime EffectiveDate,
    DateTime? ExpiryDate,
    DefaultIdType TaxCollectedAccountId,
    DefaultIdType? TaxPaidAccountId,
    string? TaxAuthority,
    string? TaxRegistrationNumber,
    string? ReportingCategory,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DefaultIdType? CreatedBy,
    DateTimeOffset? LastModifiedOn,
    DefaultIdType? LastModifiedBy);


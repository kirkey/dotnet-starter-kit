namespace FSH.Starter.WebApi.Catalog.Application.Brands.Update.v1;

/// <summary>
/// Response for updating an existing brand.
/// Contains the unique identifier of the updated brand.
/// </summary>
public sealed record UpdateBrandResponse(
    /// <summary>The unique identifier of the updated brand.</summary>
    DefaultIdType? Id);

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Create.v1;

/// <summary>
/// Response for creating a new brand.
/// Contains the unique identifier of the newly created brand.
/// </summary>
public sealed record CreateBrandResponse(
    /// <summary>The unique identifier of the newly created brand.</summary>
    DefaultIdType? Id);


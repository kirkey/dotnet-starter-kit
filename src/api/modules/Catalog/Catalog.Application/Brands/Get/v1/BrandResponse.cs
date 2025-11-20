namespace FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;

/// <summary>
/// Response containing brand information.
/// Used when retrieving a single brand entity.
/// </summary>
public sealed record BrandResponse(
    /// <summary>The unique identifier of the brand.</summary>
    DefaultIdType? Id,
    /// <summary>The name of the brand.</summary>
    string Name,
    /// <summary>The optional description of the brand.</summary>
    string? Description,
    /// <summary>Optional notes associated with the brand.</summary>
    string? Notes);

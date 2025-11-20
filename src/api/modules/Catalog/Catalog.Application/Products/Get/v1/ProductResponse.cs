using FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Get.v1;

/// <summary>
/// Response containing product information.
/// Used when retrieving a single product entity.
/// </summary>
public sealed record ProductResponse(
    /// <summary>The unique identifier of the product.</summary>
    DefaultIdType? Id,
    /// <summary>The name of the product.</summary>
    string Name,
    /// <summary>The optional description of the product.</summary>
    string? Description,
    /// <summary>The price of the product.</summary>
    decimal Price,
    /// <summary>The optional brand information associated with the product.</summary>
    BrandResponse? Brand);

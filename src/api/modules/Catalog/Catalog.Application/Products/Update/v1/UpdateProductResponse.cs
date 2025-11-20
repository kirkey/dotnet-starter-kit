namespace FSH.Starter.WebApi.Catalog.Application.Products.Update.v1;

/// <summary>
/// Response for updating an existing product.
/// Contains the unique identifier of the updated product.
/// </summary>
public sealed record UpdateProductResponse(
    /// <summary>The unique identifier of the updated product.</summary>
    DefaultIdType? Id);

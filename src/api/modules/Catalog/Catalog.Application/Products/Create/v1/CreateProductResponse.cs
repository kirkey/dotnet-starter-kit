﻿namespace FSH.Starter.WebApi.Catalog.Application.Products.Create.v1;

/// <summary>
/// Response for creating a new product.
/// Contains the unique identifier of the newly created product.
/// </summary>
public sealed record CreateProductResponse(
    /// <summary>The unique identifier of the newly created product.</summary>
    DefaultIdType? Id);


﻿using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Create.v1;

/// <summary>
/// Handler for creating a new product.
/// Validates input, creates the product entity, and persists it to the database.
/// </summary>
public sealed class CreateProductHandler(
    ILogger<CreateProductHandler> logger,
    [FromKeyedServices("catalog:products")] IRepository<Product> repository)
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    /// <summary>
    /// Handles the CreateProductCommand request.
    /// </summary>
    /// <param name="request">The create product command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A response containing the newly created product ID.</returns>
    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var product = Product.Create(request.Name!, request.Description, request.Price, request.BrandId);
        await repository.AddAsync(product, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Product created with ID: {ProductId}, Name: {ProductName}, Price: {Price}", 
            product.Id, product.Name, product.Price);
        
        return new CreateProductResponse(product.Id);
    }
}

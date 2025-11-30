using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Create.v1;

/// <summary>
/// Handler for creating a new brand.
/// Validates input, creates the brand entity, and persists it to the database.
/// </summary>
public sealed class CreateBrandHandler(
    ILogger<CreateBrandHandler> logger,
    [FromKeyedServices("catalog:brands")] IRepository<Brand> repository)
    : IRequestHandler<CreateBrandCommand, CreateBrandResponse>
{
    /// <summary>
    /// Handles the CreateBrandCommand request.
    /// </summary>
    /// <param name="request">The create brand command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A response containing the newly created brand ID.</returns>
    public async Task<CreateBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var brand = Brand.Create(request.Name!, request.Description, request.Notes);
        await repository.AddAsync(brand, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Brand created with ID: {BrandId}, Name: {BrandName}", brand.Id, brand.Name);
        
        return new CreateBrandResponse(brand.Id);
    }
}

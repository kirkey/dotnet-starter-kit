using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Domain;
using FSH.Starter.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Update.v1;

/// <summary>
/// Handler for updating an existing brand.
/// Validates input, retrieves the brand, updates it, and persists changes.
/// </summary>
public sealed class UpdateBrandHandler(
    ILogger<UpdateBrandHandler> logger,
    [FromKeyedServices("catalog:brands")] IRepository<Brand> repository)
    : IRequestHandler<UpdateBrandCommand, UpdateBrandResponse>
{
    /// <summary>
    /// Handles the UpdateBrandCommand request.
    /// </summary>
    /// <param name="request">The update brand command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A response containing the updated brand ID.</returns>
    public async Task<UpdateBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var brand = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = brand ?? throw new BrandNotFoundException(request.Id);
        
        var updatedBrand = brand.Update(request.Name, request.Description, request.Notes);
        await repository.UpdateAsync(updatedBrand, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Brand updated with ID: {BrandId}, Name: {BrandName}", brand.Id, brand.Name);
        
        return new UpdateBrandResponse(brand.Id);
    }
}

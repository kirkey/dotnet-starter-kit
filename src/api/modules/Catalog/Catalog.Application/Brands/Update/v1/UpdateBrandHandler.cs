using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Domain;
using FSH.Starter.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Update.v1;
public sealed class UpdateBrandHandler(
    ILogger<UpdateBrandHandler> logger,
    [FromKeyedServices("catalog:brands")] IRepository<Brand> repository)
    : IRequestHandler<UpdateBrandCommand, UpdateBrandResponse>
{
    public async Task<UpdateBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var brand = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = brand ?? throw new BrandNotFoundException(request.Id);
        var updatedBrand = brand.Update(request.Name, request.Description, request.Notes);
        await repository.UpdateAsync(updatedBrand, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Brand with id : {BrandId} updated.", brand.Id);
        return new UpdateBrandResponse(brand.Id);
    }
}

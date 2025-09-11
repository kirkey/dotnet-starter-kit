using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Products.Update.v1;

public sealed class UpdateProductHandler(
    ILogger<UpdateProductHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Product> repository)
    : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
{
    public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new ProductNotFoundException(request.Id);
        entity.Update(request.Name, request.SKU, request.Barcode, request.Brand, request.CostPrice, request.SellingPrice, request.Weight, request.Unit, request.ReorderLevel, request.MaxStockLevel, request.IsPerishable, request.ShelfLifeDays, request.RequiresBatchTracking, request.IsActive, request.CategoryId);
        await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("product updated {ProductId}", entity.Id);
        return new UpdateProductResponse(entity.Id);
    }
}


using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Products.Create.v1;

public sealed class CreateProductHandler(
    ILogger<CreateProductHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Product> repository)
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = Product.Create(request.Name, request.SKU, request.Barcode, request.Brand, request.CostPrice, request.SellingPrice, request.Weight, request.Unit, request.ReorderLevel, request.MaxStockLevel, request.IsPerishable, request.ShelfLifeDays, request.RequiresBatchTracking, request.IsActive, request.CategoryId);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("product created {ProductId}", entity.Id);
        return new CreateProductResponse(entity.Id);
    }
}


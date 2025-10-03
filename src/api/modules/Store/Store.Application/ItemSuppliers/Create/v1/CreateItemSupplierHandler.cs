using Store.Domain.Exceptions.ItemSupplier;

namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Create.v1;

/// <summary>
/// Handler for creating an item-supplier relationship.
/// </summary>
public sealed class CreateItemSupplierHandler(
    [FromKeyedServices("store:itemsuppliers")] IRepository<ItemSupplier> repository,
    [FromKeyedServices("store:itemsuppliers")] IReadRepository<ItemSupplier> readRepository)
    : IRequestHandler<CreateItemSupplierCommand, CreateItemSupplierResponse>
{
    public async Task<CreateItemSupplierResponse> Handle(CreateItemSupplierCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate item-supplier relationship
        var existing = await readRepository.FirstOrDefaultAsync(
            new Specs.ItemSupplierByItemAndSupplierSpec(request.ItemId, request.SupplierId),
            cancellationToken);

        if (existing is not null)
        {
            throw new DuplicateItemSupplierException(request.ItemId, request.SupplierId);
        }

        var itemSupplier = ItemSupplier.Create(
            request.ItemId,
            request.SupplierId,
            request.UnitCost,
            request.LeadTimeDays,
            request.MinimumOrderQuantity,
            request.SupplierPartNumber,
            request.PackagingQuantity,
            request.CurrencyCode ?? "USD",
            request.IsPreferred);

        await repository.AddAsync(itemSupplier, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateItemSupplierResponse(itemSupplier.Id);
    }
}

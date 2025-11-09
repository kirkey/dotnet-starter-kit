using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Specs;
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
            new ItemSupplierByItemAndSupplierSpec(request.ItemId, request.SupplierId),
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
            request.IsPreferred);

        // Set inherited AuditableEntity properties
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            itemSupplier.Name = request.Name;
        }
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            itemSupplier.Description = request.Description;
        }
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            itemSupplier.Notes = request.Notes;
        }

        await repository.AddAsync(itemSupplier, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateItemSupplierResponse(itemSupplier.Id);
    }
}

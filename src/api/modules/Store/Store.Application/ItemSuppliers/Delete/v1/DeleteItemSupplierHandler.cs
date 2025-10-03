using Store.Domain.Exceptions.ItemSupplier;

namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Delete.v1;

/// <summary>
/// Handler for deleting an item-supplier relationship.
/// </summary>
public sealed class DeleteItemSupplierHandler(
    [FromKeyedServices("store:itemsuppliers")] IRepository<ItemSupplier> repository,
    [FromKeyedServices("store:itemsuppliers")] IReadRepository<ItemSupplier> readRepository)
    : IRequestHandler<DeleteItemSupplierCommand>
{
    public async Task Handle(DeleteItemSupplierCommand request, CancellationToken cancellationToken)
    {
        var itemSupplier = await readRepository.GetByIdAsync(request.Id, cancellationToken);

        if (itemSupplier is null)
        {
            throw new ItemSupplierNotFoundException(request.Id);
        }

        await repository.DeleteAsync(itemSupplier, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

using Store.Domain.Exceptions.ItemSupplier;

namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;

/// <summary>
/// Handler for getting an item-supplier relationship by ID.
/// </summary>
public sealed class GetItemSupplierHandler(
    [FromKeyedServices("store:itemsuppliers")] IReadRepository<ItemSupplier> repository)
    : IRequestHandler<GetItemSupplierCommand, ItemSupplierResponse>
{
    public async Task<ItemSupplierResponse> Handle(GetItemSupplierCommand request, CancellationToken cancellationToken)
    {
        var itemSupplier = await repository.FirstOrDefaultAsync(
            new Specs.GetItemSupplierByIdSpec(request.Id),
            cancellationToken);

        if (itemSupplier is null)
        {
            throw new ItemSupplierNotFoundException(request.Id);
        }

        return itemSupplier;
    }
}

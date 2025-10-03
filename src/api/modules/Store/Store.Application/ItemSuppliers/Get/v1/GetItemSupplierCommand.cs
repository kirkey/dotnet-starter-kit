namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;

/// <summary>
/// Command to get an item-supplier relationship by ID.
/// </summary>
public sealed record GetItemSupplierCommand(
    DefaultIdType Id
) : IRequest<ItemSupplierResponse>;

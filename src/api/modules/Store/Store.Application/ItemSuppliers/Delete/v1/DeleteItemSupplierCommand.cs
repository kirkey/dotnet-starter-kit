namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Delete.v1;

/// <summary>
/// Command to delete an item-supplier relationship.
/// </summary>
public sealed record DeleteItemSupplierCommand(
    DefaultIdType Id
) : IRequest;

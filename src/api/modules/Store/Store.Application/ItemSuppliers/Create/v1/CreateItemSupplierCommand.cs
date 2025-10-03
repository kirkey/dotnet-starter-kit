namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Create.v1;

/// <summary>
/// Command to create a new item-supplier relationship.
/// </summary>
public sealed record CreateItemSupplierCommand(
    DefaultIdType ItemId,
    DefaultIdType SupplierId,
    decimal UnitCost,
    int LeadTimeDays,
    int MinimumOrderQuantity,
    string? SupplierPartNumber,
    int? PackagingQuantity,
    bool IsPreferred
) : IRequest<CreateItemSupplierResponse>;

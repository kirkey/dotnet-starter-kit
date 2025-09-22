namespace FSH.Starter.WebApi.Store.Application.Suppliers.Deactivate.v1;

/// <summary>
/// Command to deactivate a Supplier.
/// </summary>
public sealed record DeactivateSupplierCommand(DefaultIdType Id) : IRequest<DeactivateSupplierResponse>;


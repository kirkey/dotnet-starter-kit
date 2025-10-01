namespace FSH.Starter.WebApi.Store.Application.Suppliers.Activate.v1;

/// <summary>
/// Command to activate a Supplier.
/// </summary>
public sealed record ActivateSupplierCommand(DefaultIdType Id) : IRequest<ActivateSupplierResponse>;


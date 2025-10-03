namespace FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

/// <summary>
/// Query command to retrieve a supplier by identifier.
/// </summary>
/// <param name="Id">The supplier identifier.</param>
public sealed record GetSupplierCommand(DefaultIdType Id) : IRequest<SupplierResponse>;

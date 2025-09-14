namespace FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

public sealed record GetSupplierRequest(DefaultIdType Id) : IRequest<SupplierResponse>;


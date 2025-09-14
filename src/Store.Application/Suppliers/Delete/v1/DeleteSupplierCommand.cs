namespace FSH.Starter.WebApi.Store.Application.Suppliers.Delete.v1;

public sealed record DeleteSupplierCommand(DefaultIdType Id) : IRequest;


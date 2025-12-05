namespace Accounting.Application.Vendors.Delete.v1;

public record DeleteVendorCommand(DefaultIdType Id) : IRequest;


using MediatR;

namespace Accounting.Application.Vendors.Delete.v1;

public record VendorDeleteCommand(DefaultIdType Id) : IRequest;


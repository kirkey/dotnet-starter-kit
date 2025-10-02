// Vendor Exceptions

using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

public sealed class VendorByIdNotFoundException(DefaultIdType id) : NotFoundException($"vendor with id {id} not found");
public sealed class VendorByCodeNotFoundException(string vendorCode) : NotFoundException($"vendor with code {vendorCode} not found");
public sealed class VendorAlreadyActiveException(DefaultIdType id) : ForbiddenException($"vendor with id {id} is already active");
public sealed class VendorAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"vendor with id {id} is already inactive");
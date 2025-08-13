using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Vendors.Exceptions;

public class VendorNotFoundException(DefaultIdType id) : FshException($"Vendor with ID {id} was not found.");

public class VendorCodeAlreadyExistsException(string code)
    : ConflictException($"Vendor with code '{code}' already exists.");

public class VendorNameAlreadyExistsException(string name)
    : ConflictException($"Vendor with name '{name}' already exists.");

public class VendorAlreadyActiveException(DefaultIdType id)
    : BadRequestException($"Vendor with ID {id} is already active.");

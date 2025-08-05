using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Vendors.Exceptions;

public class VendorNotFoundException : FshException
{
    public VendorNotFoundException(DefaultIdType id) : base($"Vendor with ID {id} was not found.")
    {
    }
}

public class VendorCodeAlreadyExistsException : ConflictException
{
    public VendorCodeAlreadyExistsException(string code) : base($"Vendor with code '{code}' already exists.")
    {
    }
}

public class VendorNameAlreadyExistsException : ConflictException
{
    public VendorNameAlreadyExistsException(string name) : base($"Vendor with name '{name}' already exists.")
    {
    }
}

public class VendorAlreadyActiveException : BadRequestException
{
    public VendorAlreadyActiveException(DefaultIdType id) : base($"Vendor with ID {id} is already active.")
    {
    }
}

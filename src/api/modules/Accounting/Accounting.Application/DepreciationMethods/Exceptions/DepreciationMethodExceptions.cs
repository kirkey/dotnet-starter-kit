using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.DepreciationMethods.Exceptions;

public class DepreciationMethodNotFoundException : NotFoundException
{
    public DepreciationMethodNotFoundException(DefaultIdType id) : base($"Depreciation Method with ID {id} was not found.")
    {
    }
}

public class DepreciationMethodCodeAlreadyExistsException : ConflictException
{
    public DepreciationMethodCodeAlreadyExistsException(string code) : base($"Depreciation Method with code '{code}' already exists.")
    {
    }
}

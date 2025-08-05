using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.FixedAssets.Exceptions;

public class FixedAssetNotFoundException : NotFoundException
{
    public FixedAssetNotFoundException(DefaultIdType id) : base($"Fixed Asset with ID {id} was not found.")
    {
    }
}

public class FixedAssetAlreadyDisposedException : ForbiddenException
{
    public FixedAssetAlreadyDisposedException(DefaultIdType id) : base($"Fixed Asset with ID {id} is already disposed.")
    {
    }
}

public class FixedAssetCannotBeModifiedException : ForbiddenException
{
    public FixedAssetCannotBeModifiedException(DefaultIdType id) : base($"Fixed Asset with ID {id} cannot be modified.")
    {
    }
}

public class InvalidAssetPurchasePriceException : ForbiddenException
{
    public InvalidAssetPurchasePriceException() : base("Asset purchase price must be greater than zero.")
    {
    }
}

public class InvalidAssetServiceLifeException : ForbiddenException
{
    public InvalidAssetServiceLifeException() : base("Asset service life must be greater than zero.")
    {
    }
}

public class InvalidAssetSalvageValueException : ForbiddenException
{
    public InvalidAssetSalvageValueException() : base("Salvage value must be less than purchase price and greater than or equal to zero.")
    {
    }
}

public class InvalidDepreciationAmountException : ForbiddenException
{
    public InvalidDepreciationAmountException() : base("Depreciation amount must be greater than zero.")
    {
    }
}

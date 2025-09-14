namespace Accounting.Application.FixedAssets.Exceptions;

public class FixedAssetNotFoundException(DefaultIdType id)
    : NotFoundException($"Fixed Asset with ID {id} was not found.");

public class FixedAssetAlreadyDisposedException(DefaultIdType id)
    : ForbiddenException($"Fixed Asset with ID {id} is already disposed.");

public class FixedAssetCannotBeModifiedException(DefaultIdType id)
    : ForbiddenException($"Fixed Asset with ID {id} cannot be modified.");

public class InvalidAssetPurchasePriceException()
    : ForbiddenException("Asset purchase price must be greater than zero.");

public class InvalidAssetServiceLifeException() : ForbiddenException("Asset service life must be greater than zero.");

public class InvalidAssetSalvageValueException()
    : ForbiddenException("Salvage value must be less than purchase price and greater than or equal to zero.");

public class InvalidDepreciationAmountException() : ForbiddenException("Depreciation amount must be greater than zero.");

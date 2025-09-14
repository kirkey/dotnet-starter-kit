// FixedAssetExceptions.cs
// Per-domain exceptions for Fixed Assets and related validations

namespace Accounting.Domain.Exceptions;

public sealed class FixedAssetNotFoundException(DefaultIdType id) : NotFoundException($"fixed asset with id {id} not found");
public sealed class FixedAssetAlreadyDisposedException(DefaultIdType id) : ForbiddenException($"fixed asset with id {id} is already disposed");
public sealed class FixedAssetCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"fixed asset with id {id} cannot be modified after disposal");
public sealed class InvalidDepreciationAmountException() : ForbiddenException("depreciation amount must be positive");
public sealed class InvalidAssetPurchasePriceException() : ForbiddenException("asset purchase price must be positive");
public sealed class InvalidAssetServiceLifeException() : ForbiddenException("asset service life must be positive");
public sealed class InvalidAssetSalvageValueException() : ForbiddenException("salvage value cannot be negative or greater than purchase price");


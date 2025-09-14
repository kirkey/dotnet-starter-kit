using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

// JournalEntry Exceptions
public sealed class JournalEntryNotFoundException(DefaultIdType id) : NotFoundException($"journal entry with id {id} not found");
public sealed class JournalEntryNotBalancedException(DefaultIdType id) : ForbiddenException($"journal entry with id {id} is not balanced");
public sealed class JournalEntryAlreadyPostedException(DefaultIdType id) : ForbiddenException($"journal entry with id {id} is already posted");
public sealed class JournalEntryCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"journal entry with id {id} cannot be modified after posting");
public sealed class JournalEntryLineNotFoundException(DefaultIdType lineId) : NotFoundException($"journal entry line with id {lineId} not found");
public sealed class InvalidJournalEntryLineAmountException() : ForbiddenException("journal entry line must have either debit or credit amount, but not both");

// GeneralLedger Exceptions
public sealed class GeneralLedgerNotFoundException(DefaultIdType id) : NotFoundException($"general ledger entry with id {id} not found");
public sealed class InvalidGeneralLedgerAmountException(string message) : ForbiddenException(message);
public sealed class InvalidUsoaClassException(string message) : ForbiddenException(message);

// AccountingPeriod Exceptions
public sealed class AccountingPeriodNotFoundException(DefaultIdType id) : NotFoundException($"accounting period with id {id} not found");
public sealed class AccountingPeriodAlreadyClosedException(DefaultIdType id) : ForbiddenException($"accounting period with id {id} is already closed");
public sealed class AccountingPeriodNotClosedException(DefaultIdType id) : ForbiddenException($"accounting period with id {id} is not closed");
public sealed class AccountingPeriodCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"accounting period with id {id} cannot be modified after closing");
public sealed class InvalidAccountingPeriodDateRangeException() : ForbiddenException("accounting period start date must be before end date");
// New domain validation exceptions for AccountingPeriod
public sealed class AccountingPeriodInvalidNameException(string message) : ForbiddenException(message);
public sealed class AccountingPeriodInvalidPeriodTypeException(string periodType) : ForbiddenException($"invalid accounting period type '{periodType}'");
public sealed class AccountingPeriodInvalidFiscalYearException(int year) : ForbiddenException($"invalid fiscal year '{year}'");

// ChartOfAccount Exceptions
public sealed class ChartOfAccountInvalidException(string message) : ForbiddenException(message);

// FixedAsset Exceptions
public sealed class FixedAssetNotFoundException(DefaultIdType id) : NotFoundException($"fixed asset with id {id} not found");
public sealed class FixedAssetAlreadyDisposedException(DefaultIdType id) : ForbiddenException($"fixed asset with id {id} is already disposed");
public sealed class FixedAssetCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"fixed asset with id {id} cannot be modified after disposal");
public sealed class InvalidDepreciationAmountException() : ForbiddenException("depreciation amount must be positive");
public sealed class InvalidAssetPurchasePriceException() : ForbiddenException("asset purchase price must be positive");
public sealed class InvalidAssetServiceLifeException() : ForbiddenException("asset service life must be positive");
public sealed class InvalidAssetSalvageValueException() : ForbiddenException("salvage value cannot be negative or greater than purchase price");

// DepreciationMethod Exceptions
public sealed class DepreciationMethodNotFoundException(DefaultIdType id) : NotFoundException($"depreciation method with id {id} not found");
public sealed class DepreciationMethodAlreadyActiveException(DefaultIdType id) : ForbiddenException($"depreciation method with id {id} is already active");
public sealed class DepreciationMethodAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"depreciation method with id {id} is already inactive");

// Member/Customer Exceptions  
public sealed class MemberNotFoundException(DefaultIdType id) : NotFoundException($"member with id {id} not found");
public sealed class MemberAccountInactiveException(DefaultIdType id) : ForbiddenException($"member account with id {id} is inactive");
public sealed class InvalidMeterReadingException() : ForbiddenException("meter reading cannot be negative");

// Power Company Specific Exceptions
public sealed class ConsumptionDataNotFoundException(DefaultIdType id) : NotFoundException($"consumption data with id {id} not found");
public sealed class FuelConsumptionNotFoundException(DefaultIdType id) : NotFoundException($"fuel consumption record with id {id} not found");
public sealed class InvalidFuelTypeException(string message) : ForbiddenException(message);

// Fuel Consumption Exceptions
public sealed class InvalidFuelQuantityException(string message) : ForbiddenException(message);
public sealed class InvalidFuelCostException(string message) : ForbiddenException(message);

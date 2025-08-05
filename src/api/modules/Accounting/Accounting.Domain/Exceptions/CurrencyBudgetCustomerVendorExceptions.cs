using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

// Currency Exceptions
public sealed class CurrencyByIdNotFoundException(DefaultIdType id) : NotFoundException($"currency with id {id} not found");
public sealed class CurrencyByCodeNotFoundException(string currencyCode) : NotFoundException($"currency with code {currencyCode} not found");
public sealed class CurrencyAlreadyActiveException(DefaultIdType id) : ForbiddenException($"currency with id {id} is already active");
public sealed class CurrencyAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"currency with id {id} is already inactive");
public sealed class CurrencyAlreadyBaseCurrencyException(DefaultIdType id) : ForbiddenException($"currency with id {id} is already the base currency");
public sealed class CurrencyNotBaseCurrencyException(DefaultIdType id) : ForbiddenException($"currency with id {id} is not the base currency");
public sealed class CannotDeactivateBaseCurrencyException(DefaultIdType id) : ForbiddenException($"cannot deactivate base currency with id {id}");
public sealed class InvalidCurrencyCodeException(string code) : ForbiddenException($"invalid currency code: {code}. Must be a 3-character ISO code");
public sealed class InvalidCurrencyDecimalPlacesException(int places) : ForbiddenException($"invalid decimal places: {places}. Must be between 0 and 4");

// ExchangeRate Exceptions
public sealed class ExchangeRateNotFoundException(DefaultIdType id) : NotFoundException($"exchange rate with id {id} not found");
public sealed class ExchangeRateAlreadyActiveException(DefaultIdType id) : ForbiddenException($"exchange rate with id {id} is already active");
public sealed class ExchangeRateAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"exchange rate with id {id} is already inactive");
public sealed class InvalidExchangeRateException(decimal rate) : ForbiddenException($"exchange rate must be positive, got: {rate}");
public sealed class SameCurrencyExchangeRateException() : ForbiddenException("from and to currencies cannot be the same");

// Budget Exceptions
public sealed class BudgetNotFoundException(DefaultIdType id) : NotFoundException($"budget with id {id} not found");
public sealed class BudgetAlreadyApprovedException(DefaultIdType id) : ForbiddenException($"budget with id {id} is already approved");
public sealed class BudgetNotApprovedException(DefaultIdType id) : ForbiddenException($"budget with id {id} is not approved");
public sealed class BudgetCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"budget with id {id} cannot be modified after approval");
public sealed class BudgetLineNotFoundException(DefaultIdType budgetId, DefaultIdType accountId) : NotFoundException($"budget line for account {accountId} in budget {budgetId} not found");
public sealed class BudgetLineAlreadyExistsException(DefaultIdType budgetId, DefaultIdType accountId) : ForbiddenException($"budget line for account {accountId} already exists in budget {budgetId}");
public sealed class InvalidBudgetAmountException() : ForbiddenException("budget amount cannot be negative");
public sealed class EmptyBudgetCannotBeApprovedException(DefaultIdType id) : ForbiddenException($"cannot approve budget {id} with no budget lines");

// Customer Exceptions
public sealed class CustomerByIdNotFoundException(DefaultIdType id) : NotFoundException($"customer with id {id} not found");
public sealed class CustomerByCodeNotFoundException(string customerCode) : NotFoundException($"customer with code {customerCode} not found");
public sealed class CustomerAlreadyActiveException(DefaultIdType id) : ForbiddenException($"customer with id {id} is already active");
public sealed class CustomerAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"customer with id {id} is already inactive");
public sealed class InvalidCustomerCreditLimitException() : ForbiddenException("customer credit limit cannot be negative");
public sealed class CustomerCreditLimitExceededException(DefaultIdType customerId, decimal currentBalance, decimal creditLimit) 
    : ForbiddenException($"customer {customerId} credit limit exceeded. Current balance: {currentBalance}, Credit limit: {creditLimit}");
public sealed class InvalidCustomerBalanceTransactionException() : ForbiddenException("customer balance transaction amount must be positive");

// Vendor Exceptions
public sealed class VendorByIdNotFoundException(DefaultIdType id) : NotFoundException($"vendor with id {id} not found");
public sealed class VendorByCodeNotFoundException(string vendorCode) : NotFoundException($"vendor with code {vendorCode} not found");
public sealed class VendorAlreadyActiveException(DefaultIdType id) : ForbiddenException($"vendor with id {id} is already active");
public sealed class VendorAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"vendor with id {id} is already inactive");

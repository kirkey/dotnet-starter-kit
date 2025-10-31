// Accounts Payable Account Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class APAccountByIdNotFoundException(DefaultIdType id) : NotFoundException($"AP account with id {id} not found");

public sealed class APAccountByNumberNotFoundException(string accountNumber) : NotFoundException($"AP account with number {accountNumber} not found");

public sealed class DuplicateAPAccountNumberException(string accountNumber) : ConflictException($"AP account with number {accountNumber} already exists");

public sealed class InvalidAPAgingBucketException() : ForbiddenException("AP aging bucket values cannot be negative");

public sealed class InvalidAPPaymentAmountException() : ForbiddenException("payment amount must be positive");

public sealed class InvalidDiscountAmountException() : ForbiddenException("discount amount must be positive");


// Accounts Payable Account Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class ApAccountByIdNotFoundException(DefaultIdType id) : NotFoundException($"AP account with id {id} not found");

public sealed class ApAccountByNumberNotFoundException(string accountNumber) : NotFoundException($"AP account with number {accountNumber} not found");

public sealed class DuplicateApAccountNumberException(string accountNumber) : ConflictException($"AP account with number {accountNumber} already exists");

public sealed class InvalidApAgingBucketException() : ForbiddenException("AP aging bucket values cannot be negative");

public sealed class InvalidApPaymentAmountException() : ForbiddenException("payment amount must be positive");

public sealed class InvalidDiscountAmountException() : ForbiddenException("discount amount must be positive");


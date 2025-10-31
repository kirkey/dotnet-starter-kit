// Accounts Receivable Account Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when an AR account is not found by ID.
/// </summary>
public sealed class ARAccountByIdNotFoundException(DefaultIdType id) : NotFoundException($"AR account with id {id} not found");

/// <summary>
/// Exception thrown when an AR account is not found by account number.
/// </summary>
public sealed class ARAccountByNumberNotFoundException(string accountNumber) : NotFoundException($"AR account with number {accountNumber} not found");

/// <summary>
/// Exception thrown when trying to create an AR account with a duplicate number.
/// </summary>
public sealed class DuplicateARAccountNumberException(string accountNumber) : ConflictException($"AR account with number {accountNumber} already exists");

/// <summary>
/// Exception thrown when aging bucket values are negative.
/// </summary>
public sealed class InvalidAgingBucketException() : ForbiddenException("aging bucket values cannot be negative");

/// <summary>
/// Exception thrown when allowance exceeds current balance.
/// </summary>
public sealed class AllowanceExceedsBalanceException(decimal allowance, decimal balance) : ForbiddenException($"allowance {allowance:C} cannot exceed AR balance {balance:C}");

/// <summary>
/// Exception thrown when write-off amount is invalid.
/// </summary>
public sealed class InvalidWriteOffAmountException() : ForbiddenException("write-off amount must be positive");

/// <summary>
/// Exception thrown when collection amount is invalid.
/// </summary>
public sealed class InvalidCollectionAmountException() : ForbiddenException("collection amount must be positive");

/// <summary>
/// Exception thrown when reconciliation variance is significant.
/// </summary>
public sealed class SignificantReconciliationVarianceException(decimal variance) : ForbiddenException($"reconciliation variance of {variance:C} requires investigation");


// Security Deposit Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a security deposit is not found by ID.
/// </summary>
public sealed class SecurityDepositByIdNotFoundException(DefaultIdType id) : NotFoundException($"security deposit with id {id} not found");

/// <summary>
/// Exception thrown when a security deposit is not found by member ID.
/// </summary>
public sealed class SecurityDepositByMemberNotFoundException(DefaultIdType memberId) : NotFoundException($"security deposit for member {memberId} not found");

/// <summary>
/// Exception thrown when trying to create a duplicate security deposit for a member.
/// </summary>
public sealed class DuplicateSecurityDepositException(DefaultIdType memberId) : ConflictException($"security deposit for member {memberId} already exists");

/// <summary>
/// Exception thrown when a security deposit amount is invalid (negative or zero).
/// </summary>
public sealed class InvalidSecurityDepositAmountException() : ForbiddenException("security deposit amount must be positive");

/// <summary>
/// Exception thrown when trying to refund more than the available deposit plus interest.
/// </summary>
public sealed class RefundExceedsAvailableAmountException(decimal availableAmount, decimal requestedRefund) 
    : ForbiddenException($"refund amount {requestedRefund:C} exceeds available amount {availableAmount:C}");

/// <summary>
/// Exception thrown when trying to modify an already refunded deposit.
/// </summary>
public sealed class CannotModifyRefundedDepositException(DefaultIdType id) : ForbiddenException($"cannot modify refunded security deposit with id {id}");

/// <summary>
/// Exception thrown when the deposit date is invalid.
/// </summary>
public sealed class InvalidDepositDateException() : ForbiddenException("deposit date cannot be in the future");

/// <summary>
/// Exception thrown when the interest rate is invalid.
/// </summary>
public sealed class InvalidInterestRateException() : ForbiddenException("interest rate cannot be negative");

/// <summary>
/// Exception thrown when trying to transfer a deposit that has outstanding obligations.
/// </summary>
public sealed class CannotTransferDepositWithObligationsException(DefaultIdType id) : ForbiddenException($"cannot transfer security deposit with id {id} that has outstanding obligations");

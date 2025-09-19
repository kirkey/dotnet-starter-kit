namespace Accounting.Domain.Events.SecurityDeposit;

/// <summary>
/// Event raised when a security deposit is created.
/// </summary>
public record SecurityDepositCreated(
    DefaultIdType Id,
    DefaultIdType MemberId,
    decimal DepositAmount,
    DateTime DepositDate,
    decimal? InterestRate) : DomainEvent;

/// <summary>
/// Event raised when a security deposit is updated.
/// </summary>
public record SecurityDepositUpdated(Accounting.Domain.SecurityDeposit SecurityDeposit) : DomainEvent;

/// <summary>
/// Event raised when a security deposit is deleted.
/// </summary>
public record SecurityDepositDeleted(
    DefaultIdType Id,
    DefaultIdType MemberId,
    decimal DepositAmount) : DomainEvent;

/// <summary>
/// Event raised when a security deposit is refunded.
/// </summary>
public record SecurityDepositRefunded(
    DefaultIdType Id,
    DefaultIdType MemberId,
    decimal DepositAmount,
    decimal RefundAmount,
    DateTime RefundDate,
    string? RefundReason) : DomainEvent;

/// <summary>
/// Event raised when interest is accrued on a security deposit.
/// </summary>
public record SecurityDepositInterestAccrued(
    DefaultIdType Id,
    DefaultIdType MemberId,
    decimal AccruedInterest,
    decimal TotalAccruedInterest,
    DateTime AccrualDate) : DomainEvent;

/// <summary>
/// Event raised when a security deposit is transferred to a new member.
/// </summary>
public record SecurityDepositTransferred(
    DefaultIdType Id,
    DefaultIdType FromMemberId,
    DefaultIdType ToMemberId,
    decimal DepositAmount,
    DateTime TransferDate,
    string? TransferReason) : DomainEvent;

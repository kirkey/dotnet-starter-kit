// Power Purchase Agreement Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a power purchase agreement is not found by ID.
/// </summary>
public sealed class PowerPurchaseAgreementByIdNotFoundException(DefaultIdType id) : NotFoundException($"power purchase agreement with id {id} not found");

/// <summary>
/// Exception thrown when a power purchase agreement is not found by contract number.
/// </summary>
public sealed class PowerPurchaseAgreementByContractNumberNotFoundException(string contractNumber) : NotFoundException($"power purchase agreement with contract number {contractNumber} not found");

/// <summary>
/// Exception thrown when trying to create a power purchase agreement with a duplicate contract number.
/// </summary>
public sealed class DuplicatePowerPurchaseAgreementNumberException(string contractNumber) : ConflictException($"power purchase agreement with contract number {contractNumber} already exists");

/// <summary>
/// Exception thrown when trying to modify a terminated contract.
/// </summary>
public sealed class CannotModifyTerminatedContractException(DefaultIdType id) : ForbiddenException($"cannot modify terminated power purchase agreement with id {id}");

/// <summary>
/// Exception thrown when the end date is not after the start date.
/// </summary>
public sealed class InvalidContractEndDateException() : ForbiddenException("contract end date must be after start date");

/// <summary>
/// Exception thrown when energy price or demand charge is negative.
/// </summary>
public sealed class InvalidContractPriceException() : ForbiddenException("contract prices cannot be negative");

/// <summary>
/// Exception thrown when minimum purchase exceeds maximum purchase.
/// </summary>
public sealed class InvalidPurchaseLimitsException() : ForbiddenException("minimum purchase cannot exceed maximum purchase");

/// <summary>
/// Exception thrown when trying to record settlement for a non-active contract.
/// </summary>
public sealed class CannotSettleInactiveContractException(DefaultIdType id, string currentStatus) : ForbiddenException($"can only record settlements for active contracts, current status is {currentStatus}");

/// <summary>
/// Exception thrown when settlement amounts are negative.
/// </summary>
public sealed class InvalidSettlementAmountException() : ForbiddenException("settlement energy and amount cannot be negative");

/// <summary>
/// Exception thrown when trying to apply escalation to a contract without escalation terms.
/// </summary>
public sealed class NoEscalationTermsException(DefaultIdType id) : ForbiddenException($"power purchase agreement {id} does not have price escalation terms");

/// <summary>
/// Exception thrown when trying to escalate before the scheduled date.
/// </summary>
public sealed class EscalationNotYetDueException(DateTime nextEscalationDate) : ForbiddenException($"price escalation not due until {nextEscalationDate:yyyy-MM-dd}");

/// <summary>
/// Exception thrown when trying to mark a contract as expired before its end date.
/// </summary>
public sealed class ContractNotExpiredYetException(DateTime endDate) : ForbiddenException($"contract does not expire until {endDate:yyyy-MM-dd}");

/// <summary>
/// Exception thrown when contract capacity is invalid.
/// </summary>
public sealed class InvalidContractCapacityException() : ForbiddenException("contract capacity must be positive if specified");

/// <summary>
/// Exception thrown when a counterparty already has an active contract.
/// </summary>
public sealed class CounterpartyAlreadyHasActiveContractException(string counterpartyName) : ConflictException($"counterparty {counterpartyName} already has an active power purchase agreement");


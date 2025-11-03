// Interconnection Agreement Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when an interconnection agreement is not found by ID.
/// </summary>
public sealed class InterconnectionAgreementByIdNotFoundException(DefaultIdType id) : NotFoundException($"interconnection agreement with id {id} not found");

/// <summary>
/// Exception thrown when an interconnection agreement is not found by agreement number.
/// </summary>
public sealed class InterconnectionAgreementByNumberNotFoundException(string agreementNumber) : NotFoundException($"interconnection agreement with number {agreementNumber} not found");

/// <summary>
/// Exception thrown when trying to create an interconnection agreement with a duplicate agreement number.
/// </summary>
public sealed class DuplicateInterconnectionAgreementNumberException(string agreementNumber) : ConflictException($"interconnection agreement with number {agreementNumber} already exists");

/// <summary>
/// Exception thrown when trying to modify a terminated interconnection agreement.
/// </summary>
public sealed class CannotModifyTerminatedInterconnectionAgreementException(DefaultIdType id) : ForbiddenException($"cannot modify terminated interconnection agreement with id {id}");

/// <summary>
/// Exception thrown when generation exceeds annual limit.
/// </summary>
public sealed class GenerationExceedsAnnualLimitException(decimal generation, decimal limit) : ForbiddenException($"generation {generation:N2} kWh exceeds annual limit of {limit:N2} kWh");

/// <summary>
/// Exception thrown when trying to record generation for inactive agreement.
/// </summary>
public sealed class CannotRecordGenerationForInactiveAgreementException(DefaultIdType id) : ForbiddenException($"cannot record generation for inactive interconnection agreement with id {id}");

/// <summary>
/// Exception thrown when credit amount exceeds available balance.
/// </summary>
public sealed class CreditExceedsAvailableBalanceException(decimal creditAmount, decimal availableBalance) : ForbiddenException($"credit amount {creditAmount:N2} exceeds available balance {availableBalance:N2}");

/// <summary>
/// Exception thrown when installed capacity is not positive.
/// </summary>
public sealed class InstalledCapacityMustBePositiveException() : ForbiddenException("installed capacity must be positive");

/// <summary>
/// Exception thrown when rates are negative.
/// </summary>
public sealed class RateCannotBeNegativeException(string rateName) : ForbiddenException($"{rateName} cannot be negative");

/// <summary>
/// Exception thrown when trying to suspend terminated agreement.
/// </summary>
public sealed class CannotSuspendTerminatedAgreementException(DefaultIdType id) : ForbiddenException($"cannot suspend terminated interconnection agreement with id {id}");

/// <summary>
/// Exception thrown when trying to activate terminated agreement.
/// </summary>
public sealed class CannotActivateTerminatedAgreementException(DefaultIdType id) : ForbiddenException($"cannot activate terminated interconnection agreement with id {id}");

/// <summary>
/// Exception thrown when agreement is already terminated.
/// </summary>
public sealed class InterconnectionAgreementAlreadyTerminatedException(DefaultIdType id) : ForbiddenException($"interconnection agreement with id {id} is already terminated");

/// <summary>
/// Exception thrown when generation amount is not positive.
/// </summary>
public sealed class GenerationAmountMustBePositiveException() : ForbiddenException("generation amount must be positive");

/// <summary>
/// Exception thrown when credit amount is not positive.
/// </summary>
public sealed class CreditAmountMustBePositiveException() : ForbiddenException("credit amount must be positive");


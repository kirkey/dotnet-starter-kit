// Inter-Company Transaction Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when an inter-company transaction is not found by ID.
/// </summary>
public sealed class InterCompanyTransactionByIdNotFoundException(DefaultIdType id) : NotFoundException($"inter-company transaction with id {id} not found");

/// <summary>
/// Exception thrown when an inter-company transaction is not found by transaction number.
/// </summary>
public sealed class InterCompanyTransactionByNumberNotFoundException(string transactionNumber) : NotFoundException($"inter-company transaction with number {transactionNumber} not found");

/// <summary>
/// Exception thrown when trying to create an inter-company transaction with a duplicate number.
/// </summary>
public sealed class DuplicateInterCompanyTransactionNumberException(string transactionNumber) : ConflictException($"inter-company transaction with number {transactionNumber} already exists");

/// <summary>
/// Exception thrown when from entity and to entity are the same.
/// </summary>
public sealed class SameEntityTransactionException() : ForbiddenException("inter-company transactions must involve different entities");

/// <summary>
/// Exception thrown when trying to modify a reconciled inter-company transaction.
/// </summary>
public sealed class CannotModifyReconciledInterCompanyTransactionException(DefaultIdType id) : ForbiddenException($"cannot modify reconciled inter-company transaction with id {id}");

/// <summary>
/// Exception thrown when transaction amount is invalid (non-positive).
/// </summary>
public sealed class InvalidInterCompanyAmountException() : ForbiddenException("inter-company transaction amount must be positive");

/// <summary>
/// Exception thrown when due date is before transaction date.
/// </summary>
public sealed class InvalidInterCompanyDueDateException() : ForbiddenException("due date cannot be before transaction date");

/// <summary>
/// Exception thrown when trying to reconcile a non-matched transaction.
/// </summary>
public sealed class CannotReconcileUnmatchedTransactionException(DefaultIdType id) : ForbiddenException($"inter-company transaction {id} must be matched before reconciliation");

/// <summary>
/// Exception thrown when trying to mark a non-pending transaction as matched.
/// </summary>
public sealed class TransactionAlreadyMatchedException(DefaultIdType id, string currentStatus) : ForbiddenException($"inter-company transaction {id} is already {currentStatus}");

/// <summary>
/// Exception thrown when trying to reverse an eliminated transaction.
/// </summary>
public sealed class CannotReverseEliminatedTransactionException(DefaultIdType id) : ForbiddenException($"cannot reverse eliminated inter-company transaction {id}");

/// <summary>
/// Exception thrown when trying to settle an unreconciled transaction.
/// </summary>
public sealed class CannotSettleUnreconciledTransactionException(DefaultIdType id) : ForbiddenException($"inter-company transaction {id} must be reconciled before settlement");

/// <summary>
/// Exception thrown when trying to post elimination for a transaction that doesn't require it.
/// </summary>
public sealed class EliminationNotRequiredException(DefaultIdType id) : ForbiddenException($"inter-company transaction {id} does not require elimination");

/// <summary>
/// Exception thrown when elimination entry already posted.
/// </summary>
public sealed class EliminationAlreadyPostedException(DefaultIdType id) : ForbiddenException($"elimination entry already posted for inter-company transaction {id}");

/// <summary>
/// Exception thrown when trying to close an unreconciled transaction.
/// </summary>
public sealed class CannotCloseUnreconciledInterCompanyTransactionException(DefaultIdType id) : ForbiddenException($"can only close reconciled inter-company transactions");

/// <summary>
/// Exception thrown when trying to resolve a dispute for a non-disputed transaction.
/// </summary>
public sealed class TransactionNotDisputedException(DefaultIdType id) : ForbiddenException($"inter-company transaction {id} is not disputed");

/// <summary>
/// Exception thrown when matching transaction amounts don't match.
/// </summary>
public sealed class InterCompanyAmountMismatchException(decimal amount1, decimal amount2) : ConflictException($"inter-company transaction amounts do not match: {amount1:N2} vs {amount2:N2}");


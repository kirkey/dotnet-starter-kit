namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception raised when account reconciliation is not found.
/// </summary>
public sealed class AccountReconciliationNotFoundException(DefaultIdType id)
    : NotFoundException($"Account reconciliation with ID {id} was not found.");

/// <summary>
/// Exception raised when attempting to update an approved reconciliation.
/// </summary>
public sealed class CannotUpdateApprovedReconciliationException(DefaultIdType id)
    : BadRequestException($"Cannot update approved reconciliation {id}.");

/// <summary>
/// Exception raised when attempting to approve reconciliation with variance.
/// </summary>
public sealed class CannotApproveReconciliationWithVarianceException(DefaultIdType id, decimal variance)
    : BadRequestException($"Cannot approve reconciliation {id} with variance {variance:C}. Must be zero.");

/// <summary>
/// Exception raised when invalid reconciliation status transition.
/// </summary>
public sealed class InvalidReconciliationStatusTransitionException(string currentStatus, string requestedStatus)
    : BadRequestException($"Cannot transition from {currentStatus} to {requestedStatus}.");

/// <summary>
/// Exception raised when duplicate reconciliation for period.
/// </summary>
public sealed class DuplicateReconciliationException(DefaultIdType glAccountId, DefaultIdType periodId)
    : ConflictException($"Reconciliation already exists for GL account {glAccountId} in period {periodId}.");

/// <summary>
/// Exception raised when balances are invalid.
/// </summary>
public sealed class InvalidReconciliationBalanceException(string message)
    : BadRequestException(message);


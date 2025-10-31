// Trial Balance Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class TrialBalanceByIdNotFoundException(DefaultIdType id) : NotFoundException($"trial balance with id {id} not found");

public sealed class TrialBalanceByNumberNotFoundException(string trialBalanceNumber) : NotFoundException($"trial balance with number {trialBalanceNumber} not found");

public sealed class DuplicateTrialBalanceNumberException(string trialBalanceNumber) : ConflictException($"trial balance with number {trialBalanceNumber} already exists");

public sealed class CannotModifyFinalizedTrialBalanceException(DefaultIdType id) : ForbiddenException($"cannot modify finalized trial balance with id {id}");

public sealed class CannotFinalizeUnbalancedTrialBalanceException(decimal outOfBalanceAmount) : ForbiddenException($"cannot finalize trial balance that is out of balance by {outOfBalanceAmount:C}");

public sealed class AccountingEquationDoesNotBalanceException(decimal assets, decimal liabilities, decimal equity) : ForbiddenException($"accounting equation does not balance: Assets ({assets:C}) ≠ Liabilities ({liabilities:C}) + Equity ({equity:C})");

public sealed class TrialBalanceAlreadyFinalizedException(DefaultIdType id) : ForbiddenException($"trial balance with id {id} is already finalized");

public sealed class CanOnlyReopenFinalizedTrialBalanceException() : ForbiddenException("can only reopen finalized trial balances");

public sealed class InvalidTrialBalanceLineItemException() : ForbiddenException("trial balance line item values are invalid");


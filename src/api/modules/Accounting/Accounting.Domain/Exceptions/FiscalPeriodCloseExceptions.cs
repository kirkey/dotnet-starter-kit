// Fiscal Period Close Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class FiscalPeriodCloseByIdNotFoundException(DefaultIdType id) : NotFoundException($"fiscal period close with id {id} not found");

public sealed class FiscalPeriodCloseByNumberNotFoundException(string closeNumber) : NotFoundException($"fiscal period close with number {closeNumber} not found");

public sealed class DuplicateFiscalPeriodCloseNumberException(string closeNumber) : ConflictException($"fiscal period close with number {closeNumber} already exists");

public sealed class CannotModifyCompletedPeriodCloseException(DefaultIdType id) : ForbiddenException($"cannot modify completed period close with id {id}");

public sealed class CannotCompleteWithPendingTasksException(int remainingTasks) : ForbiddenException($"cannot complete period close with {remainingTasks} required tasks remaining");

public sealed class CannotCompleteWithUnbalancedTrialBalanceException() : ForbiddenException("cannot complete period close with unbalanced trial balance");

public sealed class CannotCompleteWithUnresolvedCriticalIssuesException() : ForbiddenException("cannot complete period close with unresolved critical validation issues");

public sealed class YearEndCloseRequiresNetIncomeTransferException() : ForbiddenException("year-end close requires net income to be transferred to retained earnings");

public sealed class CanOnlyReopenCompletedPeriodCloseException() : ForbiddenException("can only reopen completed period closes");

public sealed class InvalidCloseTypeException(string closeType) : ForbiddenException($"invalid close type: {closeType}. Must be MonthEnd, QuarterEnd, or YearEnd");

public sealed class CloseTaskNotFoundException(string taskName) : NotFoundException($"close task '{taskName}' not found");

public sealed class ValidationIssueNotFoundException(string issueDescription) : NotFoundException($"validation issue '{issueDescription}' not found or already resolved");

public sealed class PeriodCloseAlreadyCompletedException(DefaultIdType id) : ForbiddenException($"period close with id {id} is already completed");

public sealed class NetIncomeTransferOnlyForYearEndException() : ForbiddenException("net income transfer is only applicable for year-end close");


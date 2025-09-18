// JournalEntry Exceptions (moved out of CoreAccountingExceptions.cs for clarity)

namespace Accounting.Domain.Exceptions;

public sealed class JournalEntryNotFoundException(DefaultIdType id) : NotFoundException($"journal entry with id {id} not found");
public sealed class JournalEntryNotBalancedException(DefaultIdType id) : ForbiddenException($"journal entry with id {id} is not balanced");
public sealed class JournalEntryAlreadyPostedException(DefaultIdType id) : ForbiddenException($"journal entry with id {id} is already posted");
public sealed class JournalEntryCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"journal entry with id {id} cannot be modified after posting");
public sealed class JournalEntryLineNotFoundException(DefaultIdType lineId) : NotFoundException($"journal entry line with id {lineId} not found");
public sealed class InvalidJournalEntryLineAmountException() : ForbiddenException("journal entry line must have either debit or credit amount, but not both");
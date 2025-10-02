namespace Accounting.Domain.Exceptions;

public sealed class RecurringJournalEntryNotFoundException(DefaultIdType id) 
    : NotFoundException($"Recurring journal entry with id {id} not found");

public sealed class RecurringJournalEntryAlreadyApprovedException(DefaultIdType id) 
    : ForbiddenException($"Recurring journal entry {id} has already been approved");

public sealed class RecurringJournalEntryExpiredException(DefaultIdType id) 
    : ForbiddenException($"Recurring journal entry {id} has expired");

public sealed class RecurringJournalEntryInactiveException(DefaultIdType id) 
    : ForbiddenException($"Recurring journal entry {id} is inactive");

public sealed class InvalidRecurringEntryStatusException(string message) 
    : ForbiddenException(message);

public sealed class InvalidRecurrenceFrequencyException(string message) 
    : BadRequestException(message);

public sealed class RecurringJournalEntryCannotBeModifiedException(DefaultIdType id) 
    : ForbiddenException($"Recurring journal entry {id} cannot be modified");

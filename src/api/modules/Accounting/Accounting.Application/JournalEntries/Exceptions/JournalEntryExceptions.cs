using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.JournalEntries.Exceptions;

public class JournalEntryNotFoundException(DefaultIdType id)
    : NotFoundException($"Journal Entry with ID {id} was not found.");

public class JournalEntryReferenceNumberAlreadyExistsException(string referenceNumber)
    : ConflictException($"Journal Entry with reference number '{referenceNumber}' already exists.");

public class JournalEntryAlreadyPostedException(DefaultIdType id)
    : BadRequestException($"Journal Entry with ID {id} is already posted and cannot be modified.");

public class InvalidJournalEntryException()
    : BadRequestException("Journal Entry must have balanced debits and credits.");

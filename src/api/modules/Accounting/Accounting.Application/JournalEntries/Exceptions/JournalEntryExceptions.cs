using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.JournalEntries.Exceptions;

public class JournalEntryNotFoundException : NotFoundException
{
    public JournalEntryNotFoundException(DefaultIdType id) : base($"Journal Entry with ID {id} was not found.")
    {
    }
}

public class JournalEntryReferenceNumberAlreadyExistsException : ConflictException
{
    public JournalEntryReferenceNumberAlreadyExistsException(string referenceNumber) : base($"Journal Entry with reference number '{referenceNumber}' already exists.")
    {
    }
}

public class JournalEntryAlreadyPostedException : BadRequestException
{
    public JournalEntryAlreadyPostedException(DefaultIdType id) : base($"Journal Entry with ID {id} is already posted and cannot be modified.")
    {
    }
}

public class InvalidJournalEntryException : BadRequestException
{
    public InvalidJournalEntryException() : base("Journal Entry must have balanced debits and credits.")
    {
    }
}

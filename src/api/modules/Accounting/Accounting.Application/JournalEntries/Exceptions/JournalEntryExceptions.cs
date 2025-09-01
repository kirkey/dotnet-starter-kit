using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.JournalEntries.Exceptions;

public class JournalEntryAlreadyPostedException : ConflictException
{
    public JournalEntryAlreadyPostedException(string message) : base(message)
    {
    }
}

public class JournalEntryUnbalancedException : BadRequestException
{
    public JournalEntryUnbalancedException(string message) : base(message)
    {
    }
}

public class JournalEntryNotFoundException : NotFoundException
{
    public JournalEntryNotFoundException(string message) : base(message)
    {
    }
}

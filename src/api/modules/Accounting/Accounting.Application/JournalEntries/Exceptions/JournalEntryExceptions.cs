using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.JournalEntries.Exceptions;

public class JournalEntryAlreadyPostedException(string message) : ConflictException(message);

public class JournalEntryUnbalancedException(string message) : BadRequestException(message);

public class JournalEntryNotFoundException(string message) : NotFoundException(message);

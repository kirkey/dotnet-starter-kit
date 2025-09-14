// ChartOfAccountExceptions.cs
// Per-domain exceptions for Chart of Accounts (non-not-found cases)

namespace Accounting.Domain.Exceptions;

public sealed class ChartOfAccountInvalidException(string message) : ForbiddenException(message);

// Not-found exception grouped here for Chart of Accounts
public sealed class ChartOfAccountNotFoundException(DefaultIdType id) : NotFoundException($"account with id {id} not found");

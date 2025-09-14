// GeneralLedgerExceptions.cs
// Per-domain exceptions for the General Ledger

namespace Accounting.Domain.Exceptions;

public sealed class GeneralLedgerNotFoundException(DefaultIdType id) : NotFoundException($"general ledger entry with id {id} not found");
public sealed class InvalidGeneralLedgerAmountException(string message) : ForbiddenException(message);
public sealed class InvalidUsoaClassException(string message) : ForbiddenException(message);


using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;
public sealed class AccountNotFoundException(DefaultIdType id) : NotFoundException($"account with id {id} not found");

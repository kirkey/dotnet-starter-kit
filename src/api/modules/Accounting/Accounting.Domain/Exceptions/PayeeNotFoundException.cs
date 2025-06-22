using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

public sealed class PayeeNotFoundException(DefaultIdType id)
    : NotFoundException($"payee with id {id} not found");

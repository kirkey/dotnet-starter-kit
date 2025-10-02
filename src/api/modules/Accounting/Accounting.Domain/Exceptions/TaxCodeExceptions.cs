using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

public sealed class TaxCodeNotFoundException(DefaultIdType id) 
    : NotFoundException($"Tax code with id {id} not found");

public sealed class TaxCodeAlreadyActiveException(DefaultIdType id) 
    : ForbiddenException($"Tax code {id} is already active");

public sealed class TaxCodeAlreadyInactiveException(DefaultIdType id) 
    : ForbiddenException($"Tax code {id} is already inactive");

public sealed class TaxCodeInactiveException(DefaultIdType id) 
    : ForbiddenException($"Tax code {id} is inactive and cannot be used");

public sealed class InvalidTaxRateException(string message) 
    : BadRequestException(message);

public sealed class TaxCodeInUseException(DefaultIdType id) 
    : ForbiddenException($"Tax code {id} is in use and cannot be modified or deleted");

public sealed class InvalidTaxTypeException(string message) 
    : BadRequestException(message);

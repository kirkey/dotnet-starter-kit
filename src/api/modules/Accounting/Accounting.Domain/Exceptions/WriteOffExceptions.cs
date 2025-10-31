namespace Accounting.Domain.Exceptions;

public sealed class WriteOffNotFoundException(DefaultIdType id) 
    : NotFoundException($"Write-off with id {id} not found");

public sealed class WriteOffCannotBeModifiedException(DefaultIdType id) 
    : ForbiddenException($"Write-off {id} has been approved and cannot be modified");

public sealed class WriteOffAlreadyApprovedException(DefaultIdType id) 
    : ForbiddenException($"Write-off {id} has already been approved");

public sealed class WriteOffNotApprovedException(DefaultIdType id) 
    : ForbiddenException($"Write-off {id} has not been approved");

public sealed class InvalidWriteOffStatusException(string message) 
    : ForbiddenException(message);

public sealed class WriteOffAmountException(string message) 
    : BadRequestException(message);

public sealed class WriteOffRecoveryExceedsAmountException(DefaultIdType id) 
    : BadRequestException($"Recovery amount exceeds original write-off amount for {id}");

public sealed class WriteOffCannotBeReversedException(DefaultIdType id) 
    : ForbiddenException($"Write-off {id} cannot be reversed");

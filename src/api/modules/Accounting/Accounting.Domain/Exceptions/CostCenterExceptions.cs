namespace Accounting.Domain.Exceptions;

public sealed class CostCenterNotFoundException(DefaultIdType id) 
    : NotFoundException($"Cost center with id {id} not found");

public sealed class CostCenterAlreadyActiveException(DefaultIdType id) 
    : ForbiddenException($"Cost center {id} is already active");

public sealed class CostCenterAlreadyInactiveException(DefaultIdType id) 
    : ForbiddenException($"Cost center {id} is already inactive");

public sealed class CostCenterInactiveException(DefaultIdType id) 
    : ForbiddenException($"Cost center {id} is inactive and cannot be used");

public sealed class CostCenterInUseException(DefaultIdType id) 
    : ForbiddenException($"Cost center {id} has transactions and cannot be deleted");

public sealed class InvalidCostCenterBudgetException(string message) 
    : BadRequestException(message);

public sealed class CostCenterCodeAlreadyExistsException(string code) 
    : ForbiddenException($"Cost center with code {code} already exists");

public sealed class InvalidCostCenterTypeException(string costCenterType) 
    : BadRequestException($"Invalid cost center type: {costCenterType}. Must be one of: Department, Division, BusinessUnit, Project, Location, Other");


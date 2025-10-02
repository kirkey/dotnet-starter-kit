namespace Accounting.Application.Projects.Costing.Exceptions;

/// <summary>
/// Exception thrown when a project costing entry is not found.
/// </summary>
public class ProjectCostEntryNotFoundException(DefaultIdType id)
    : NotFoundException($"Project costing entry with ID {id} was not found.");

/// <summary>
/// Exception thrown when attempting to modify an approved project costing entry.
/// </summary>
public class ApprovedProjectCostingCannotBeModifiedException(DefaultIdType id)
    : ForbiddenException($"Project costing entry with ID {id} cannot be modified because it has been approved.");

/// <summary>
/// Exception thrown when attempting to delete an approved project costing entry.
/// </summary>
public class ApprovedProjectCostingCannotBeDeletedException(DefaultIdType id)
    : ForbiddenException($"Project costing entry with ID {id} cannot be deleted because it has been approved.");

/// <summary>
/// Exception thrown when project cost description is required but not provided.
/// </summary>
public class ProjectCostDescriptionRequiredException()
    : BadRequestException("Project cost description is required and cannot be empty.");

/// <summary>
/// Exception thrown when an invalid project cost amount is provided.
/// </summary>
public class InvalidProjectCostAmountException()
    : BadRequestException("Project cost amount must be positive.");

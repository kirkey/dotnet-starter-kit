// Project Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when an invalid project budget amount is provided.
/// </summary>
public class InvalidProjectBudgetException() : BadRequestException("Project budget amount must be non-negative.");

/// <summary>
/// Exception thrown when attempting to modify a project that cannot be modified (completed or cancelled).
/// </summary>
public class ProjectCannotBeModifiedException(DefaultIdType projectId)
    : ForbiddenException($"Project {projectId} cannot be modified because it is completed or cancelled.");

/// <summary>
/// Exception thrown when an invalid project cost entry amount is provided.
/// </summary>
public class InvalidProjectCostEntryException() : BadRequestException("Project cost entry amount must be positive.");

/// <summary>
/// Exception thrown when an invalid project revenue entry amount is provided.
/// </summary>
public class InvalidProjectRevenueEntryException()
    : BadRequestException("Project revenue entry amount must be positive.");

/// <summary>
/// Exception thrown when a project name is required but not provided.
/// </summary>
public class ProjectNameRequiredException() : BadRequestException("Project name is required and cannot be empty.");

/// <summary>
/// Exception thrown when project start date is invalid (in the future).
/// </summary>
public class InvalidProjectStartDateException() : BadRequestException("Project start date cannot be in the future.");

/// <summary>
/// Exception thrown when project end date is invalid (before start date).
/// </summary>
public class InvalidProjectEndDateException() : BadRequestException("Project end date must be after start date.");

/// <summary>
/// Exception thrown when attempting to complete a project without setting end date.
/// </summary>
public class ProjectCompletionDateRequiredException()
    : BadRequestException("Project completion date is required when marking project as completed.");

/// <summary>
/// Exception thrown when a project with duplicate name/code already exists.
/// </summary>
public class DuplicateProjectException(string projectName)
    : ConflictException($"A project with name '{projectName}' already exists.");

/// <summary>
/// Exception thrown when a project is not found.
/// </summary>
public class ProjectNotFoundException(DefaultIdType projectId)
    : NotFoundException($"Project with ID {projectId} was not found.");

/// <summary>
/// Exception thrown when project cost amount exceeds budget without authorization.
/// </summary>
public class ProjectCostExceedsBudgetException(decimal budgetAmount, decimal totalCost) : BadRequestException(
    $"Project total cost ({totalCost:C}) exceeds approved budget ({budgetAmount:C}) without authorization.");

/// <summary>
/// Exception thrown when attempting to complete a project that is already completed.
/// </summary>
public class ProjectAlreadyCompletedException(DefaultIdType projectId)
    : ConflictException($"Project {projectId} is already completed.");

/// <summary>
/// Exception thrown when attempting to cancel a project that is already cancelled or completed.
/// </summary>
public class ProjectAlreadyCancelledException(DefaultIdType projectId)
    : ConflictException($"Project {projectId} is already cancelled or completed.");

// ProjectCost specific exceptions

/// <summary>
/// Exception thrown when an invalid project cost amount is provided.
/// </summary>
public class InvalidProjectCostAmountException() : BadRequestException("Project cost amount must be positive.");

/// <summary>
/// Exception thrown when project cost description is required but not provided.
/// </summary>
public class ProjectCostDescriptionRequiredException()
    : BadRequestException("Project cost description is required and cannot be empty.");

/// <summary>
/// Exception thrown when project cost entry date is invalid (in the future).
/// </summary>
public class InvalidProjectCostDateException() : BadRequestException("Project cost entry date cannot be in the future.");

/// <summary>
/// Exception thrown when attempting to modify an approved project cost entry.
/// </summary>
public class ApprovedProjectCostCannotBeModifiedException(DefaultIdType projectCostId)
    : ConflictException($"Project cost entry {projectCostId} cannot be modified because it has been approved.");

/// <summary>
/// Exception thrown when a project cost entry is not found.
/// </summary>
public class ProjectCostNotFoundException(DefaultIdType projectCostId)
    : NotFoundException($"Project cost entry with ID {projectCostId} was not found.");

/// <summary>
/// Exception thrown when an invalid cost category is provided.
/// </summary>
public class InvalidProjectCostCategoryException(string category)
    : BadRequestException($"Invalid project cost category: '{category}'.");

/// <summary>
/// Exception thrown when an invalid cost center is provided.
/// </summary>
public class InvalidCostCenterException(string costCenter)
    : BadRequestException($"Invalid cost center: '{costCenter}'.");

/// <summary>
/// Exception thrown when attempting to add costs to a project that doesn't allow it (wrong status).
/// </summary>
public class ProjectCostEntryNotAllowedException(DefaultIdType projectId)
    : ForbiddenException($"Cost entries are not allowed for project {projectId} in its current status.");

/// <summary>
/// Exception thrown when a job costing entry is not found.
/// </summary>
public class JobCostingEntryNotFoundException(DefaultIdType entryId)
    : NotFoundException($"Job costing entry with ID {entryId} was not found.");

/// <summary>
/// Exception thrown when adding a cost would exceed the project's approved budget without authorization.
/// </summary>
public class ProjectBudgetExceededException(decimal addedAmount, decimal budgetAmount, decimal currentCost)
    : BadRequestException(
        $"Adding amount {addedAmount:C} would exceed project budget {budgetAmount:C}. Current cost is {currentCost:C}.");

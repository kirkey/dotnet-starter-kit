namespace Accounting.Application.Projects.Exceptions;

public class ProjectNotFoundException(DefaultIdType id) : NotFoundException($"Project with ID {id} was not found.");

public class ProjectNameAlreadyExistsException(string name)
    : ConflictException($"Project with name '{name}' already exists.");

public class InvalidProjectBudgetException() : BadRequestException("Project budget must be greater than zero.");

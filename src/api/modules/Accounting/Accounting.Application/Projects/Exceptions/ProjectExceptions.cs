using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Projects.Exceptions;

public class ProjectNotFoundException : NotFoundException
{
    public ProjectNotFoundException(DefaultIdType id) : base($"Project with ID {id} was not found.")
    {
    }
}

public class ProjectNameAlreadyExistsException : ConflictException
{
    public ProjectNameAlreadyExistsException(string name) : base($"Project with name '{name}' already exists.")
    {
    }
}

public class InvalidProjectBudgetException : BadRequestException
{
    public InvalidProjectBudgetException() : base("Project budget must be greater than zero.")
    {
    }
}

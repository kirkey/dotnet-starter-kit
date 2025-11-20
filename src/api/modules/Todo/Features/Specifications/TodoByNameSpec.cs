using Ardalis.Specification;
using FSH.Starter.WebApi.Todo.Domain;

namespace FSH.Starter.WebApi.Todo.Features.Specifications;

/// <summary>
/// Specification to find a todo item by its name (case-insensitive).
/// Used for uniqueness validation.
/// </summary>
public sealed class TodoByNameSpec : Specification<TodoItem>
{
    public TodoByNameSpec(string name)
    {
        Query.Where(t => t.Name.ToLower() == name.ToLower());
    }
}


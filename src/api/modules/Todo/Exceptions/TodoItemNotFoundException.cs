using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Todo.Exceptions;
internal sealed class TodoItemNotFoundException : NotFoundException
{
    public TodoItemNotFoundException(DefaultIdType id)
        : base($"todo item with id {id} not found")
    {
    }
}

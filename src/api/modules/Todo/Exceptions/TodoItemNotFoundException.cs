using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Todo.Exceptions;
internal sealed class TodoItemNotFoundException(DefaultIdType id)
    : NotFoundException($"todo item with id {id} not found");

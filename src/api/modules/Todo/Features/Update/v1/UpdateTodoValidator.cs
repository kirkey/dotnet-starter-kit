using FluentValidation;
using FSH.Starter.WebApi.Todo.Persistence;

namespace FSH.Starter.WebApi.Todo.Features.Update.v1;
public class UpdateTodoValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoValidator(TodoDbContext context)
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}

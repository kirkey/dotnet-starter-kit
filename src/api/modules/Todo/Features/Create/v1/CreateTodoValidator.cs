using FluentValidation;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Todo.Domain;
using FSH.Starter.WebApi.Todo.Features.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Todo.Features.Create.v1;

/// <summary>
/// Validator for CreateTodoCommand with comprehensive validation rules.
/// Validates name length, uniqueness, and optional description/notes length.
/// </summary>
public class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoValidator(
        [FromKeyedServices("todo")] IReadRepository<TodoItem> repository)
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Task name is required.")
            .MinimumLength(TodoItem.NameMinLength)
            .MaximumLength(TodoItem.NameMaxLength)
            .MustAsync(async (name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;
                var existingTodo = await repository.FirstOrDefaultAsync(
                    new TodoByNameSpec(name.Trim()), ct).ConfigureAwait(false);
                return existingTodo is null;
            })
            .WithMessage("Task with this name already exists.");

        // Description is optional but if provided, must not exceed max length
        RuleFor(t => t.Description)
            .MaximumLength(TodoItem.DescriptionMaxLength)
            .When(t => !string.IsNullOrWhiteSpace(t.Description));

        // Notes are optional but if provided, must not exceed max length
        RuleFor(t => t.Notes)
            .MaximumLength(TodoItem.NotesMaxLength)
            .When(t => !string.IsNullOrWhiteSpace(t.Notes));
    }
}

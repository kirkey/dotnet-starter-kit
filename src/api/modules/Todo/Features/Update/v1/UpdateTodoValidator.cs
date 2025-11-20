using FluentValidation;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Todo.Domain;
using FSH.Starter.WebApi.Todo.Features.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Todo.Features.Update.v1;

/// <summary>
/// Validator for UpdateTodoCommand with comprehensive validation rules.
/// Validates name length, uniqueness (excluding current record), and optional description/notes length.
/// Allows clearing optional fields by passing empty string.
/// </summary>
public class UpdateTodoValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoValidator(
        [FromKeyedServices("todo")] IReadRepository<TodoItem> repository)
    {
        RuleFor(t => t.Id)
            .NotEmpty()
            .WithMessage("Task ID is required.");

        // Name is required and can be updated (not null means update it)
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("Task name is required.")
            .MinimumLength(TodoItem.NameMinLength)
            .MaximumLength(TodoItem.NameMaxLength)
            .MustAsync(async (command, name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;
                var existingTodo = await repository.FirstOrDefaultAsync(
                    new TodoByNameSpec(name.Trim()), ct).ConfigureAwait(false);
                return existingTodo is null || existingTodo.Id == command.Id;
            })
            .WithMessage("Task with this name already exists.");

        // Description is optional - if provided (not null), must be valid
        RuleFor(t => t.Description)
            .MaximumLength(TodoItem.DescriptionMaxLength)
            .When(t => t.Description != null && t.Description.Length > 0);

        // Notes are optional - if provided (not null), must be valid
        RuleFor(t => t.Notes)
            .MaximumLength(TodoItem.NotesMaxLength)
            .When(t => t.Notes != null && t.Notes.Length > 0);
    }
}

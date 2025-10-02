namespace Accounting.Application.Projects.Costing.Delete;

/// <summary>
/// Validator for <see cref="DeleteProjectCostingCommand"/>.
/// </summary>
public sealed class DeleteProjectCostingCommandValidator : AbstractValidator<DeleteProjectCostingCommand>
{
    public DeleteProjectCostingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project costing entry ID is required.");
    }
}

namespace Accounting.Application.Projects.Costs.Delete;

public sealed class DeleteProjectCostEntryCommandValidator : AbstractValidator<DeleteProjectCostEntryCommand>
{
    public DeleteProjectCostEntryCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.EntryId).NotEmpty();
    }
}

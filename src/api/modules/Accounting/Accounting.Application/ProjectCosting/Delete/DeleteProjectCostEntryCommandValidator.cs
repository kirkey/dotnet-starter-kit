namespace Accounting.Application.ProjectCosting.Delete;

public sealed class DeleteProjectCostEntryCommandValidator : AbstractValidator<DeleteProjectCostEntryCommand>
{
    public DeleteProjectCostEntryCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.EntryId).NotEmpty();
    }
}

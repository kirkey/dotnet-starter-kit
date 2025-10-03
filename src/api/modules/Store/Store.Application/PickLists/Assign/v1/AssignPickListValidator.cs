namespace FSH.Starter.WebApi.Store.Application.PickLists.Assign.v1;

public sealed class AssignPickListValidator : AbstractValidator<AssignPickListCommand>
{
    public AssignPickListValidator()
    {
        RuleFor(x => x.PickListId)
            .NotEmpty().WithMessage("Pick list ID is required");

        RuleFor(x => x.AssignedTo)
            .NotEmpty().WithMessage("Assigned to is required")
            .MaximumLength(100).WithMessage("Assigned to must not exceed 100 characters");
    }
}

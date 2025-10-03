namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Assign.v1;

public sealed class AssignPutAwayTaskValidator : AbstractValidator<AssignPutAwayTaskCommand>
{
    public AssignPutAwayTaskValidator()
    {
        RuleFor(x => x.PutAwayTaskId)
            .NotEmpty().WithMessage("Put-away task ID is required");

        RuleFor(x => x.AssignedTo)
            .NotEmpty().WithMessage("Assigned to is required")
            .MaximumLength(100).WithMessage("Assigned to must not exceed 100 characters");
    }
}

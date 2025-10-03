namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Delete.v1;

public sealed class DeletePutAwayTaskValidator : AbstractValidator<DeletePutAwayTaskCommand>
{
    public DeletePutAwayTaskValidator()
    {
        RuleFor(x => x.PutAwayTaskId)
            .NotEmpty().WithMessage("Put-away task ID is required");
    }
}

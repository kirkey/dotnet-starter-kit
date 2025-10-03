namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Complete.v1;

public sealed class CompletePutAwayValidator : AbstractValidator<CompletePutAwayCommand>
{
    public CompletePutAwayValidator()
    {
        RuleFor(x => x.PutAwayTaskId)
            .NotEmpty().WithMessage("Put-away task ID is required");
    }
}

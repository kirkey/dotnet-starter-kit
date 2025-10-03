namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Start.v1;

public sealed class StartPutAwayValidator : AbstractValidator<StartPutAwayCommand>
{
    public StartPutAwayValidator()
    {
        RuleFor(x => x.PutAwayTaskId)
            .NotEmpty().WithMessage("Put-away task ID is required");
    }
}

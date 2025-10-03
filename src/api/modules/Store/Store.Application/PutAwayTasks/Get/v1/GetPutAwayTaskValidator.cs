namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;

public sealed class GetPutAwayTaskValidator : AbstractValidator<GetPutAwayTaskRequest>
{
    public GetPutAwayTaskValidator()
    {
        RuleFor(x => x.PutAwayTaskId)
            .NotEmpty().WithMessage("Put-away task ID is required");
    }
}

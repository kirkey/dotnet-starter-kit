namespace FSH.Starter.WebApi.Store.Application.PickLists.Start.v1;

public sealed class StartPickingValidator : AbstractValidator<StartPickingCommand>
{
    public StartPickingValidator()
    {
        RuleFor(x => x.PickListId)
            .NotEmpty().WithMessage("Pick list ID is required");
    }
}

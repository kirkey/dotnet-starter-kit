namespace FSH.Starter.WebApi.Store.Application.PickLists.Complete.v1;

public sealed class CompletePickingValidator : AbstractValidator<CompletePickingCommand>
{
    public CompletePickingValidator()
    {
        RuleFor(x => x.PickListId)
            .NotEmpty().WithMessage("Pick list ID is required");
    }
}

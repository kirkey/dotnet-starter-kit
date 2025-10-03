namespace FSH.Starter.WebApi.Store.Application.PickLists.Delete.v1;

public sealed class DeletePickListValidator : AbstractValidator<DeletePickListCommand>
{
    public DeletePickListValidator()
    {
        RuleFor(x => x.PickListId)
            .NotEmpty().WithMessage("Pick list ID is required");
    }
}

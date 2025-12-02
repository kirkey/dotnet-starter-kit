namespace FSH.Starter.WebApi.Store.Application.PickLists.AddItem.v1;

public sealed class AddPickListItemValidator : AbstractValidator<AddPickListItemCommand>
{
    public AddPickListItemValidator()
    {
        RuleFor(x => x.PickListId)
            .NotEmpty().WithMessage("Pick list ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item ID is required");

        RuleFor(x => x.QuantityToPick)
            .GreaterThan(0).WithMessage("Quantity to pick must be positive");

        RuleFor(x => x.Notes)
            .MaximumLength(512).WithMessage("Notes must not exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

public sealed class AddPutAwayTaskItemValidator : AbstractValidator<AddPutAwayTaskItemCommand>
{
    public AddPutAwayTaskItemValidator()
    {
        RuleFor(x => x.PutAwayTaskId)
            .NotEmpty().WithMessage("Put-away task ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item ID is required");

        RuleFor(x => x.ToBinId)
            .NotEmpty().WithMessage("Destination bin ID is required");

        RuleFor(x => x.QuantityToPutAway)
            .GreaterThan(0).WithMessage("Quantity must be positive");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes must not exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

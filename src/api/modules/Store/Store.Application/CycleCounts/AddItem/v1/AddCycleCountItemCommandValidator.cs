namespace FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.v1;

public class AddCycleCountItemCommandValidator : AbstractValidator<AddCycleCountItemCommand>
{
    public AddCycleCountItemCommandValidator()
    {
        RuleFor(x => x.CycleCountId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.SystemQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CountedQuantity).GreaterThanOrEqualTo(0).When(x => x.CountedQuantity.HasValue);
    }
}


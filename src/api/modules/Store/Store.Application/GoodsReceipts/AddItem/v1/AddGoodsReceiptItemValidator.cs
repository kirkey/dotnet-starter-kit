namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.AddItem.v1;

public sealed class AddGoodsReceiptItemValidator : AbstractValidator<AddGoodsReceiptItemCommand>
{
    public AddGoodsReceiptItemValidator()
    {
        RuleFor(x => x.GoodsReceiptId)
            .NotEmpty().WithMessage("Goods receipt ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Item name is required")
            .MaximumLength(200).WithMessage("Item name must not exceed 200 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be positive");
    }
}

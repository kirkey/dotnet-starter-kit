namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Delete.v1;

public sealed class DeleteGoodsReceiptValidator : AbstractValidator<DeleteGoodsReceiptCommand>
{
    public DeleteGoodsReceiptValidator()
    {
        RuleFor(x => x.GoodsReceiptId)
            .NotEmpty().WithMessage("Goods receipt ID is required");
    }
}

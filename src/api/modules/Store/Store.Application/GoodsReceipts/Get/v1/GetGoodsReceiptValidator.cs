namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Get.v1;

public class GetGoodsReceiptValidator : AbstractValidator<GetGoodsReceiptCommand>
{
    public GetGoodsReceiptValidator()
    {
        RuleFor(x => x.GoodsReceiptId)
            .NotEmpty().WithMessage("Goods receipt ID is required");
    }
}

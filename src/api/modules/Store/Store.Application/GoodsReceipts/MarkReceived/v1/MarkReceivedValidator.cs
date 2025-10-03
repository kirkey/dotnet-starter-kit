namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.MarkReceived.v1;

public sealed class MarkReceivedValidator : AbstractValidator<MarkReceivedCommand>
{
    public MarkReceivedValidator()
    {
        RuleFor(x => x.GoodsReceiptId)
            .NotEmpty().WithMessage("Goods receipt ID is required");
    }
}

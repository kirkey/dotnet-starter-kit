namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Create.v1;

public sealed class CreateGoodsReceiptValidator : AbstractValidator<CreateGoodsReceiptCommand>
{
    public CreateGoodsReceiptValidator()
    {
        RuleFor(x => x.ReceiptNumber)
            .NotEmpty().WithMessage("Receipt number is required")
            .MaximumLength(100).WithMessage("Receipt number must not exceed 100 characters");

        RuleFor(x => x.ReceivedDate)
            .NotEmpty().WithMessage("Received date is required");
    }
}

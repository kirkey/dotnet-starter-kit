namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Create.v1;

/// <summary>
/// Validator for CreateGoodsReceiptCommand.
/// </summary>
public sealed class CreateGoodsReceiptValidator : AbstractValidator<CreateGoodsReceiptCommand>
{
    public CreateGoodsReceiptValidator()
    {
        RuleFor(x => x.ReceiptNumber)
            .NotEmpty().WithMessage("Receipt number is required")
            .NotNull().WithMessage("Receipt number cannot be null")
            .MaximumLength(100).WithMessage("Receipt number must not exceed 100 characters");

        RuleFor(x => x.ReceivedDate)
            .NotEmpty().WithMessage("Received date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).WithMessage("Received date cannot be in the future");

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.Name)
            .MaximumLength(1024).When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

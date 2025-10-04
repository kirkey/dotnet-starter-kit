namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1;

public class UpdateStockAdjustmentCommandValidator : AbstractValidator<UpdateStockAdjustmentCommand>
{
    private static readonly string[] AllowedAdjustmentTypes = new[] { "Physical Count", "Damage", "Loss", "Found", "Transfer", "Other", "Increase", "Decrease", "Write-Off" };

    public UpdateStockAdjustmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId is required");

        RuleFor(x => x.AdjustmentType)
            .NotEmpty().WithMessage("Adjustment type is required")
            .Must(t => AllowedAdjustmentTypes.Contains(t)).WithMessage($"Adjustment type must be one of: {string.Join(", ", AllowedAdjustmentTypes)}");

        RuleFor(x => x.QuantityAdjusted)
            .GreaterThan(0).WithMessage("Quantity adjusted must be greater than zero");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required")
            .MaximumLength(200).WithMessage("Reason must not exceed 200 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.Name)
            .MaximumLength(1024).WithMessage("Name must not exceed 1024 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description must not exceed 2048 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}


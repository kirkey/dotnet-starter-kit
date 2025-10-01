namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1;

public class UpdateStockAdjustmentCommandValidator : AbstractValidator<UpdateStockAdjustmentCommand>
{
    private static readonly string[] AllowedAdjustmentTypes = new[] { "Physical Count", "Damage", "Loss", "Found", "Transfer", "Other", "Increase", "Decrease", "Write-Off" };

    public UpdateStockAdjustmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.GroceryItemId).NotEmpty().WithMessage("GroceryItemId is required");

        RuleFor(x => x.AdjustmentType)
            .NotEmpty().WithMessage("Adjustment type is required")
            .Must(t => AllowedAdjustmentTypes.Contains(t)).WithMessage($"Adjustment type must be one of: {string.Join(", ", AllowedAdjustmentTypes)}");

        RuleFor(x => x.QuantityAdjusted)
            .GreaterThan(0).WithMessage("Quantity adjusted must be greater than zero");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required")
            .MaximumLength(200).WithMessage("Reason must not exceed 200 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Notes));
    }
}


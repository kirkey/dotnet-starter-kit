namespace FSH.Starter.WebApi.Store.Application.StockLevels.Create.v1;

/// <summary>
/// Validator for CreateStockLevelCommand.
/// </summary>
public sealed class CreateStockLevelCommandValidator : AbstractValidator<CreateStockLevelCommand>
{
    public CreateStockLevelCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId is required");

        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("WarehouseId is required");

        RuleFor(x => x.QuantityOnHand)
            .GreaterThanOrEqualTo(0)
            .WithMessage("QuantityOnHand cannot be negative");

        RuleFor(x => x.Name)
            .MaximumLength(1024).When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

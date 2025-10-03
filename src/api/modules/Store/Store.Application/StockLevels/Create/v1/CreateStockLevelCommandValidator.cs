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
    }
}

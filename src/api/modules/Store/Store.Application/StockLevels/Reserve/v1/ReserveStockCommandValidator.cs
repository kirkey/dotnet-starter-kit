namespace FSH.Starter.WebApi.Store.Application.StockLevels.Reserve.v1;

/// <summary>
/// Validator for ReserveStockCommand.
/// </summary>
public sealed class ReserveStockCommandValidator : AbstractValidator<ReserveStockCommand>
{
    public ReserveStockCommandValidator()
    {
        RuleFor(x => x.StockLevelId)
            .NotEmpty()
            .WithMessage("StockLevelId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be positive");
    }
}

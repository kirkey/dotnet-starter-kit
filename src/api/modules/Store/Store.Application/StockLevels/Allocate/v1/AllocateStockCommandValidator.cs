namespace FSH.Starter.WebApi.Store.Application.StockLevels.Allocate.v1;

/// <summary>
/// Validator for AllocateStockCommand.
/// </summary>
public sealed class AllocateStockCommandValidator : AbstractValidator<AllocateStockCommand>
{
    public AllocateStockCommandValidator()
    {
        RuleFor(x => x.StockLevelId)
            .NotEmpty()
            .WithMessage("StockLevelId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be positive");
    }
}

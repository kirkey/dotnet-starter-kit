namespace FSH.Starter.WebApi.Store.Application.StockLevels.Delete.v1;

/// <summary>
/// Validator for DeleteStockLevelCommand.
/// </summary>
public sealed class DeleteStockLevelCommandValidator : AbstractValidator<DeleteStockLevelCommand>
{
    public DeleteStockLevelCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}

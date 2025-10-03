namespace FSH.Starter.WebApi.Store.Application.StockLevels.Update.v1;

/// <summary>
/// Validator for UpdateStockLevelCommand.
/// </summary>
public sealed class UpdateStockLevelCommandValidator : AbstractValidator<UpdateStockLevelCommand>
{
    public UpdateStockLevelCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}

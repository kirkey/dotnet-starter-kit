namespace FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1;

/// <summary>
/// Validator for GetStockLevelRequest.
/// </summary>
public sealed class GetStockLevelCommandValidator : AbstractValidator<GetStockLevelCommand>
{
    public GetStockLevelCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}

namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1;

/// <summary>
/// Validator for GetLotNumberRequest.
/// </summary>
public sealed class GetLotNumberCommandValidator : AbstractValidator<GetLotNumberCommand>
{
    public GetLotNumberCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}

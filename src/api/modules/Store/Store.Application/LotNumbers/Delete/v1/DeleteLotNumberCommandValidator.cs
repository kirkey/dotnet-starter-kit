namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Delete.v1;

/// <summary>
/// Validator for DeleteLotNumberCommand.
/// </summary>
public sealed class DeleteLotNumberCommandValidator : AbstractValidator<DeleteLotNumberCommand>
{
    public DeleteLotNumberCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}

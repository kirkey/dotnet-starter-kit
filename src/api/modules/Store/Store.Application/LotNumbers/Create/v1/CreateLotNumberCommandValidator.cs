namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Create.v1;

/// <summary>
/// Validator for CreateLotNumberCommand.
/// </summary>
public sealed class CreateLotNumberCommandValidator : AbstractValidator<CreateLotNumberCommand>
{
    public CreateLotNumberCommandValidator()
    {
        RuleFor(x => x.LotCode)
            .NotEmpty()
            .WithMessage("LotCode is required")
            .MaximumLength(100)
            .WithMessage("LotCode must not exceed 100 characters");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId is required");

        RuleFor(x => x.QuantityReceived)
            .GreaterThanOrEqualTo(0)
            .WithMessage("QuantityReceived cannot be negative");

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(x => x.ManufactureDate)
            .WithMessage("ExpirationDate must be after ManufactureDate")
            .When(x => x.ManufactureDate.HasValue && x.ExpirationDate.HasValue);

        RuleFor(x => x.QualityNotes)
            .MaximumLength(1000)
            .WithMessage("QualityNotes must not exceed 1000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.QualityNotes));
    }
}

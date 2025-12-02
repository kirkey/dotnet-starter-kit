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
            .MaximumLength(128)
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
            .MaximumLength(1024)
            .WithMessage("QualityNotes must not exceed 1000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.QualityNotes));

        RuleFor(x => x.Name)
            .MaximumLength(1024)
            .WithMessage("Name must not exceed 1024 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .WithMessage("Description must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

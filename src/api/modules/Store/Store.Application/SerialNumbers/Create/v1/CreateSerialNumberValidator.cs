namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Create.v1;

public class CreateSerialNumberValidator : AbstractValidator<CreateSerialNumberCommand>
{
    public CreateSerialNumberValidator()
    {
        RuleFor(x => x.SerialNumberValue)
            .NotEmpty().WithMessage("Serial number value is required.")
            .MaximumLength(100).WithMessage("Serial number value must not exceed 100 characters.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item ID is required.");

        RuleFor(x => x.ExternalReference)
            .MaximumLength(100).WithMessage("External reference must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ExternalReference));

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

        RuleFor(x => x.WarrantyExpirationDate)
            .GreaterThan(x => x.ManufactureDate!.Value)
            .WithMessage("Warranty expiration date must be after manufacture date.")
            .When(x => x.ManufactureDate.HasValue && x.WarrantyExpirationDate.HasValue);

        RuleFor(x => x.ReceiptDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Receipt date cannot be in the future.")
            .When(x => x.ReceiptDate.HasValue);
    }
}

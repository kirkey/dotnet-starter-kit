namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Update.v1;

public class UpdateSerialNumberValidator : AbstractValidator<UpdateSerialNumberCommand>
{
    public UpdateSerialNumberValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Serial number ID is required.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(status => new[] { "Available", "Allocated", "Shipped", "Sold", "Defective", "Returned", "InRepair", "Scrapped" }
                .Contains(status, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Status must be one of: Available, Allocated, Shipped, Sold, Defective, Returned, InRepair, Scrapped.");

        RuleFor(x => x.ExternalReference)
            .MaximumLength(128).WithMessage("External reference must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ExternalReference));

        RuleFor(x => x.Notes)
            .MaximumLength(1024).WithMessage("Notes must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

        RuleFor(x => x.WarrantyExpirationDate)
            .GreaterThan(x => x.ManufactureDate!.Value)
            .WithMessage("Warranty expiration date must be after manufacture date.")
            .When(x => x.ManufactureDate.HasValue && x.WarrantyExpirationDate.HasValue);
    }
}

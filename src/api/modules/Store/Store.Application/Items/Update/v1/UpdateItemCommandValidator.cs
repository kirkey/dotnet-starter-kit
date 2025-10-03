namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(c => c.Name)
            .MaximumLength(200).When(c => !string.IsNullOrWhiteSpace(c.Name))
            .WithMessage("Name must not exceed 200 characters.");

        RuleFor(c => c.Sku)
            .MaximumLength(100).When(c => !string.IsNullOrWhiteSpace(c.Sku))
            .WithMessage("SKU must not exceed 100 characters.");

        RuleFor(c => c.Barcode)
            .MaximumLength(100).When(c => !string.IsNullOrWhiteSpace(c.Barcode))
            .WithMessage("Barcode must not exceed 100 characters.");

        RuleFor(c => c.UnitPrice)
            .GreaterThanOrEqualTo(0).When(c => c.UnitPrice.HasValue)
            .WithMessage("UnitPrice must be zero or greater.");

        RuleFor(c => c.Cost)
            .GreaterThanOrEqualTo(0).When(c => c.Cost.HasValue)
            .WithMessage("Cost must be zero or greater.");

        RuleFor(c => c.Brand)
            .MaximumLength(200).When(c => !string.IsNullOrWhiteSpace(c.Brand))
            .WithMessage("Brand must not exceed 200 characters.");

        RuleFor(c => c.Manufacturer)
            .MaximumLength(200).When(c => !string.IsNullOrWhiteSpace(c.Manufacturer))
            .WithMessage("Manufacturer must not exceed 200 characters.");

        RuleFor(c => c.ManufacturerPartNumber)
            .MaximumLength(100).When(c => !string.IsNullOrWhiteSpace(c.ManufacturerPartNumber))
            .WithMessage("ManufacturerPartNumber must not exceed 100 characters.");

        RuleFor(c => c.UnitOfMeasure)
            .MaximumLength(20).When(c => !string.IsNullOrWhiteSpace(c.UnitOfMeasure))
            .WithMessage("UnitOfMeasure must not exceed 20 characters.");
    }
}

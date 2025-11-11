namespace FSH.Starter.WebApi.Store.Application.SalesImports.Create.v1;

/// <summary>
/// Validator for CreateSalesImportCommand.
/// </summary>
public class CreateSalesImportCommandValidator : AbstractValidator<CreateSalesImportCommand>
{
    public CreateSalesImportCommandValidator()
    {
        RuleFor(x => x.ImportNumber)
            .NotEmpty().WithMessage("Import number is required")
            .MaximumLength(100).WithMessage("Import number must not exceed 100 characters");

        RuleFor(x => x.ImportDate)
            .NotEmpty().WithMessage("Import date is required");

        RuleFor(x => x.SalesPeriodFrom)
            .NotEmpty().WithMessage("Sales period start date is required");

        RuleFor(x => x.SalesPeriodTo)
            .NotEmpty().WithMessage("Sales period end date is required")
            .GreaterThanOrEqualTo(x => x.SalesPeriodFrom)
            .WithMessage("Sales period end date must be after or equal to start date");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required")
            .MaximumLength(255).WithMessage("File name must not exceed 255 characters");

        RuleFor(x => x.CsvData)
            .NotEmpty().WithMessage("CSV data is required");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}


namespace Accounting.Application.Payees.Import.v1;

/// <summary>
/// Validator for the ImportPayeesCommand to ensure file upload requirements are met.
/// Validates file format, size, and basic structure before processing.
/// </summary>
public sealed class ImportPayeesCommandValidator : AbstractValidator<ImportPayeesCommand>
{
    /// <summary>
    /// Initializes validation rules for Payees import command.
    /// Enforces strict file validation to prevent processing invalid data.
    /// </summary>
    public ImportPayeesCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("Import file is required");

        RuleFor(x => x.File.Name)
            .NotEmpty()
            .WithMessage("File name is required")
            .Must(fileName => fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Only Excel files (.xlsx) are supported for Payees import");

        RuleFor(x => x.File.Length)
            .GreaterThan(0)
            .WithMessage("File cannot be empty")
            .LessThanOrEqualTo(10 * 1024 * 1024) // 10MB limit
            .WithMessage("File size cannot exceed 10MB");

        RuleFor(x => x.File.ContentType)
            .Must(contentType => contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            .WithMessage("Invalid file format. Only Excel (.xlsx) files are supported");
    }
}

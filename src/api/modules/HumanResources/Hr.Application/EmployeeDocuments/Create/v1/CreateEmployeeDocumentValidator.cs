namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;

/// <summary>
/// Validator for creating an employee document.
/// </summary>
public class CreateEmployeeDocumentValidator : AbstractValidator<CreateEmployeeDocumentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEmployeeDocumentValidator"/> class.
    /// </summary>
    public CreateEmployeeDocumentValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.DocumentType)
            .NotEmpty()
            .WithMessage("Document type is required")
            .Must(BeValidDocumentType)
            .WithMessage("Document type must be Contract, Certification, License, Identity, Medical, or Other");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(256)
            .WithMessage("Title cannot exceed 250 characters");

        RuleFor(x => x.FileName)
            .MaximumLength(256)
            .WithMessage("File name cannot exceed 255 characters");

        RuleFor(x => x.FilePath)
            .MaximumLength(512)
            .WithMessage("File path cannot exceed 500 characters");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .When(x => x.FileSize.HasValue)
            .WithMessage("File size must be greater than 0");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.Today)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future");

        RuleFor(x => x.Notes)
            .MaximumLength(1024)
            .WithMessage("Notes cannot exceed 1000 characters");
    }

    /// <summary>
    /// Validates if the document type is valid.
    /// </summary>
    private static bool BeValidDocumentType(string? documentType)
    {
        if (string.IsNullOrWhiteSpace(documentType))
            return false;

        var validTypes = new[] { "Contract", "Certification", "License", "Identity", "Medical", "Other" };
        return validTypes.Contains(documentType);
    }
}

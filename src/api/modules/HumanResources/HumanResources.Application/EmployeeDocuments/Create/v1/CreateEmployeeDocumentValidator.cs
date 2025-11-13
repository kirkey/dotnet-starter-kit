namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;

public class CreateEmployeeDocumentValidator : AbstractValidator<CreateEmployeeDocumentCommand>
{
    public CreateEmployeeDocumentValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("Document type is required.")
            .Must(BeValidDocumentType).WithMessage("Document type must be one of: Contract, Certification, License, Identity, Medical, Other.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(500).WithMessage("Title must not exceed 500 characters.");

        RuleFor(x => x.FileName)
            .MaximumLength(256).WithMessage("File name must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FileName));

        RuleFor(x => x.FileSize)
            .GreaterThan(0).WithMessage("File size must be greater than zero.")
            .When(x => x.FileSize.HasValue);

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.Today).WithMessage("Expiry date must be in the future.")
            .When(x => x.ExpiryDate.HasValue);

        RuleFor(x => x.IssueNumber)
            .MaximumLength(100).WithMessage("Issue number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.IssueNumber));

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }

    private static bool BeValidDocumentType(string documentType)
    {
        return documentType is "Contract" or "Certification" or "License" or "Identity" or "Medical" or "Other";
    }
}


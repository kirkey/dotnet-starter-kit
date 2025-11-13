namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;

public class UpdateEmployeeDocumentValidator : AbstractValidator<UpdateEmployeeDocumentCommand>
{
    public UpdateEmployeeDocumentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.Title)
            .MaximumLength(500).WithMessage("Title must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.Today).WithMessage("Expiry date must be in the future.")
            .When(x => x.ExpiryDate.HasValue);

        RuleFor(x => x.IssueNumber)
            .MaximumLength(100).WithMessage("Issue number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.IssueNumber));

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

        RuleFor(x => x.FileName)
            .MaximumLength(256).WithMessage("File name must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FileName));

        RuleFor(x => x.FileSize)
            .GreaterThan(0).WithMessage("File size must be greater than zero.")
            .When(x => x.FileSize.HasValue);
    }
}


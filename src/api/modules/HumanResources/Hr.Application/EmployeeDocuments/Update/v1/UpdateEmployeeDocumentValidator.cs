namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;

/// <summary>
/// Validator for updating an employee document.
/// </summary>
public class UpdateEmployeeDocumentValidator : AbstractValidator<UpdateEmployeeDocumentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEmployeeDocumentValidator"/> class.
    /// </summary>
    public UpdateEmployeeDocumentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Document ID is required");

        RuleFor(x => x.Title)
            .MaximumLength(250)
            .WithMessage("Title cannot exceed 250 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.Today)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future");

        RuleFor(x => x.IssueNumber)
            .MaximumLength(100)
            .WithMessage("Issue number cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.IssueNumber));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}


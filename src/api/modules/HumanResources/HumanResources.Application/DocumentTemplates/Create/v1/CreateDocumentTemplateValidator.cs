namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Create.v1;

public class CreateDocumentTemplateValidator : AbstractValidator<CreateDocumentTemplateCommand>
{
    public CreateDocumentTemplateValidator()
    {
        RuleFor(x => x.TemplateName)
            .NotEmpty().WithMessage("Template name is required.")
            .MaximumLength(200).WithMessage("Template name must not exceed 200 characters.");

        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("Document type is required.")
            .MaximumLength(100).WithMessage("Document type must not exceed 100 characters.");

        RuleFor(x => x.TemplateContent)
            .NotEmpty().WithMessage("Template content is required.");

        RuleFor(x => x.TemplateVariables)
            .MaximumLength(1000).WithMessage("Template variables must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TemplateVariables));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


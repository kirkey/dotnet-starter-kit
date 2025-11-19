namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Update.v1;

public class UpdateDocumentTemplateValidator : AbstractValidator<UpdateDocumentTemplateCommand>
{
    public UpdateDocumentTemplateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.TemplateName)
            .MaximumLength(200).WithMessage("Template name must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TemplateName));

        RuleFor(x => x.TemplateContent)
            .NotEmpty().WithMessage("Template content cannot be empty.")
            .When(x => !string.IsNullOrWhiteSpace(x.TemplateContent));

        RuleFor(x => x.TemplateVariables)
            .MaximumLength(1000).WithMessage("Template variables must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TemplateVariables));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


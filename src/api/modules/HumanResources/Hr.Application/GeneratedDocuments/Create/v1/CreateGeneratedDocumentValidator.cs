namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Create.v1;

public class CreateGeneratedDocumentValidator : AbstractValidator<CreateGeneratedDocumentCommand>
{
    public CreateGeneratedDocumentValidator()
    {
        RuleFor(x => x.DocumentTemplateId)
            .NotEmpty().WithMessage("Document template ID is required.");

        RuleFor(x => x.EntityId)
            .NotEmpty().WithMessage("Entity ID is required.");

        RuleFor(x => x.EntityType)
            .NotEmpty().WithMessage("Entity type is required.")
            .MaximumLength(100).WithMessage("Entity type must not exceed 100 characters.");

        RuleFor(x => x.GeneratedContent)
            .NotEmpty().WithMessage("Generated content is required.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Update.v1;

public class UpdateGeneratedDocumentValidator : AbstractValidator<UpdateGeneratedDocumentCommand>
{
    public UpdateGeneratedDocumentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.Status)
            .Must(BeValidStatus).WithMessage("Status must be one of: Draft, Finalized, Signed, Archived.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.FilePath)
            .MaximumLength(1000).WithMessage("File path must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FilePath));

        RuleFor(x => x.SignedBy)
            .MaximumLength(200).WithMessage("Signed by must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SignedBy));

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }

    private static bool BeValidStatus(string status)
    {
        return status is "Draft" or "Finalized" or "Signed" or "Archived";
    }
}


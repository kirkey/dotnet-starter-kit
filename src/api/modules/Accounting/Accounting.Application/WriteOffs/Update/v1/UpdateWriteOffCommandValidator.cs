namespace Accounting.Application.WriteOffs.Update.v1;

public sealed class UpdateWriteOffCommandValidator : AbstractValidator<UpdateWriteOffCommand>
{
    public UpdateWriteOffCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Write-off ID is required.");
        RuleFor(x => x.Reason).MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
        RuleFor(x => x.Description).MaximumLength(2048).WithMessage("Description must not exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.Notes).MaximumLength(2048).WithMessage("Notes must not exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Reason) || 
                                  !string.IsNullOrWhiteSpace(x.Description) || 
                                  !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("At least one field must be provided for update.");
    }
}


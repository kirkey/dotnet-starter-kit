namespace Accounting.Application.WriteOffs.Reverse.v1;

public sealed class ReverseWriteOffCommandValidator : AbstractValidator<ReverseWriteOffCommand>
{
    public ReverseWriteOffCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Write-off ID is required.");
        RuleFor(x => x.Reason).MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}


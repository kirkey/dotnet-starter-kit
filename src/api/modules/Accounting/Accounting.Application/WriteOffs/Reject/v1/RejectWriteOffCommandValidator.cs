namespace Accounting.Application.WriteOffs.Reject.v1;

public sealed class RejectWriteOffCommandValidator : AbstractValidator<RejectWriteOffCommand>
{
    public RejectWriteOffCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Write-off ID is required.");
        RuleFor(x => x.RejectedBy).NotEmpty().WithMessage("Rejector information is required.")
            .MaximumLength(256).WithMessage("Rejector information must not exceed 200 characters.");
        RuleFor(x => x.Reason).MaximumLength(512).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}


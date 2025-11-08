namespace Accounting.Application.WriteOffs.Approve.v1;

public sealed class ApproveWriteOffCommandValidator : AbstractValidator<ApproveWriteOffCommand>
{
    public ApproveWriteOffCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Write-off ID is required.");
        RuleFor(x => x.ApprovedBy).NotEmpty().WithMessage("Approver information is required.")
            .MaximumLength(200).WithMessage("Approver information must not exceed 200 characters.");
    }
}


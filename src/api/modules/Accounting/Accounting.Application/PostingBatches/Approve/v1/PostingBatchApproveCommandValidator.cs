namespace Accounting.Application.PostingBatches.Approve.v1;

public sealed class PostingBatchApproveCommandValidator : AbstractValidator<PostingBatchApproveCommand>
{
    public PostingBatchApproveCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Posting batch ID is required.");
        RuleFor(x => x.ApprovedBy).NotEmpty().WithMessage("Approver information is required.")
            .MaximumLength(200).WithMessage("Approver information must not exceed 200 characters.");
    }
}


namespace Accounting.Application.PostingBatches.Reject.v1;

public sealed class PostingBatchRejectCommandValidator : AbstractValidator<PostingBatchRejectCommand>
{
    public PostingBatchRejectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Posting batch ID is required.");
        RuleFor(x => x.RejectedBy).NotEmpty().WithMessage("Rejector information is required.")
            .MaximumLength(200).WithMessage("Rejector information must not exceed 200 characters.");
    }
}


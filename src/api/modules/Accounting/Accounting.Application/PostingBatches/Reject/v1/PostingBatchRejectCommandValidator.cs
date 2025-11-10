namespace Accounting.Application.PostingBatches.Reject.v1;

public sealed class PostingBatchRejectCommandValidator : AbstractValidator<PostingBatchRejectCommand>
{
    public PostingBatchRejectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Posting batch ID is required.");

        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Reason))
            .WithMessage("Reason must not exceed 500 characters.");
    }
}


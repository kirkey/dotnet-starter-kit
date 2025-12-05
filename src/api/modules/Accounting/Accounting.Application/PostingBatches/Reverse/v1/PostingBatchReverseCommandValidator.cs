namespace Accounting.Application.PostingBatches.Reverse.v1;

public sealed class ReversePostingBatchCommandValidator : AbstractValidator<ReversePostingBatchCommand>
{
    public ReversePostingBatchCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Posting batch ID is required.");
        RuleFor(x => x.Reason).NotEmpty().WithMessage("Reason is required to reverse a posting batch.")
            .MinimumLength(10).WithMessage("Reason must be at least 10 characters.")
            .MaximumLength(512).WithMessage("Reason must not exceed 500 characters.");
    }
}


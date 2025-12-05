namespace Accounting.Application.PostingBatches.Post.v1;

public sealed class PostPostingBatchCommandValidator : AbstractValidator<PostPostingBatchCommand>
{
    public PostPostingBatchCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Posting batch ID is required.");
    }
}


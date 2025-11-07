namespace Accounting.Application.PostingBatches.Post.v1;

public sealed class PostingBatchPostCommandValidator : AbstractValidator<PostingBatchPostCommand>
{
    public PostingBatchPostCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Posting batch ID is required.");
    }
}


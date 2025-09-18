using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Validators;

public class PostingBatchCommandValidator : AbstractValidator<PostingBatchCommand>
{
    public PostingBatchCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
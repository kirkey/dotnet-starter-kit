using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Validators;

public class CreatePostingBatchCommandValidator : AbstractValidator<CreatePostingBatchCommand>
{
    public CreatePostingBatchCommandValidator()
    {
        RuleFor(x => x.BatchNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BatchDate).NotEmpty();
    }
}
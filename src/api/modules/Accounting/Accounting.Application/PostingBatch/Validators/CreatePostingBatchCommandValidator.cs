using FluentValidation;
using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Application.PostingBatch.Validators
{
    public class CreatePostingBatchCommandValidator : AbstractValidator<CreatePostingBatchCommand>
    {
        public CreatePostingBatchCommandValidator()
        {
            RuleFor(x => x.BatchNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.BatchDate).NotEmpty();
        }
    }
}


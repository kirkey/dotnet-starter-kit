using FluentValidation;
using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Application.PostingBatch.Validators
{
    public class PostPostingBatchCommandValidator : AbstractValidator<PostPostingBatchCommand>
    {
        public PostPostingBatchCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}


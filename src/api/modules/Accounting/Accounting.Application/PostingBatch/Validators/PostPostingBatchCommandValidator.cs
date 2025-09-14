using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Application.PostingBatch.Validators
{
    public class PostingBatchCommandValidator : AbstractValidator<PostingBatchCommand>
    {
        public PostingBatchCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}


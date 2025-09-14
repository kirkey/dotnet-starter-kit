using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Application.PostingBatch.Validators
{
    public class ReversePostingBatchCommandValidator : AbstractValidator<ReversePostingBatchCommand>
    {
        public ReversePostingBatchCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(200);
        }
    }
}


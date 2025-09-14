using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Application.PostingBatch.Validators
{
    public class RejectPostingBatchCommandValidator : AbstractValidator<RejectPostingBatchCommand>
    {
        public RejectPostingBatchCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.RejectedBy).NotEmpty().MaximumLength(100);
        }
    }
}


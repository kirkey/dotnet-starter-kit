using FluentValidation;
using Accounting.Application.PostingBatch.Commands;

namespace Accounting.Application.PostingBatch.Validators
{
    public class ApprovePostingBatchCommandValidator : AbstractValidator<ApprovePostingBatchCommand>
    {
        public ApprovePostingBatchCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.ApprovedBy).NotEmpty().MaximumLength(100);
        }
    }
}


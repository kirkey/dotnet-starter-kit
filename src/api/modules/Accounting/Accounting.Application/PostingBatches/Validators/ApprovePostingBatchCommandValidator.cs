using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Validators;

public class ApprovePostingBatchCommandValidator : AbstractValidator<ApprovePostingBatchCommand>
{
    public ApprovePostingBatchCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovedBy).NotEmpty().MaximumLength(100);
    }
}
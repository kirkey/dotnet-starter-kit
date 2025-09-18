using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Validators;

public class RejectPostingBatchCommandValidator : AbstractValidator<RejectPostingBatchCommand>
{
    public RejectPostingBatchCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RejectedBy).NotEmpty().MaximumLength(100);
    }
}
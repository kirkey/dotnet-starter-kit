using Accounting.Application.PostingBatches.Commands;

namespace Accounting.Application.PostingBatches.Validators;

public class ReversePostingBatchCommandValidator : AbstractValidator<ReversePostingBatchCommand>
{
    public ReversePostingBatchCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(200);
    }
}
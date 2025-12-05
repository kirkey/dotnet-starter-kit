namespace Accounting.Application.PostingBatches.Approve.v1;

/// <summary>
/// Validator for ApprovePostingBatchCommand.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApprovePostingBatchCommandValidator : AbstractValidator<ApprovePostingBatchCommand>
{
    public ApprovePostingBatchCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Posting batch ID is required.");
    }
}


namespace Accounting.Application.PostingBatches.Approve.v1;

/// <summary>
/// Validator for PostingBatchApproveCommand.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class PostingBatchApproveCommandValidator : AbstractValidator<PostingBatchApproveCommand>
{
    public PostingBatchApproveCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Posting batch ID is required.");
    }
}


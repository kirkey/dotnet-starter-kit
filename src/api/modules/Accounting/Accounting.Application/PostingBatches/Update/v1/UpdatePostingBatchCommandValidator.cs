namespace Accounting.Application.PostingBatches.Update.v1;

/// <summary>
/// Validator for UpdatePostingBatchCommand.
/// </summary>
public sealed class UpdatePostingBatchCommandValidator : AbstractValidator<UpdatePostingBatchCommand>
{
    public UpdatePostingBatchCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Posting batch ID is required.");

        RuleFor(x => x.BatchDate)
            .NotEmpty()
            .WithMessage("Batch date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(30))
            .WithMessage("Batch date cannot be more than 30 days in the future.");

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description cannot exceed 500 characters.");
    }
}


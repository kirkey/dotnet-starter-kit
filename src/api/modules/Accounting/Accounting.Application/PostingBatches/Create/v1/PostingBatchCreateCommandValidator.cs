namespace Accounting.Application.PostingBatches.Create.v1;

/// <summary>
/// Validator for PostingBatchCreateCommand.
/// </summary>
public sealed class PostingBatchCreateCommandValidator : AbstractValidator<PostingBatchCreateCommand>
{
    public PostingBatchCreateCommandValidator()
    {
        RuleFor(x => x.BatchNumber)
            .NotEmpty()
            .WithMessage("Batch number is required.")
            .MaximumLength(64)
            .WithMessage("Batch number must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-]+$")
            .WithMessage("Batch number can only contain letters, numbers, and hyphens.");

        RuleFor(x => x.BatchDate)
            .NotEmpty()
            .WithMessage("Batch date is required.");

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}


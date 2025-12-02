namespace Accounting.Application.FixedAssets.Reject;

/// <summary>
/// Validator for RejectFixedAssetCommand.
/// </summary>
public sealed class RejectFixedAssetCommandValidator : AbstractValidator<RejectFixedAssetCommand>
{
    public RejectFixedAssetCommandValidator()
    {
        RuleFor(x => x.FixedAssetId)
            .NotEmpty()
            .WithMessage("Fixed Asset ID is required.");

        RuleFor(x => x.RejectedBy)
            .NotEmpty()
            .WithMessage("Rejector is required.")
            .MaximumLength(256)
            .WithMessage("Rejector name cannot exceed 200 characters.");

        RuleFor(x => x.Reason)
            .MaximumLength(512)
            .WithMessage("Reason cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}


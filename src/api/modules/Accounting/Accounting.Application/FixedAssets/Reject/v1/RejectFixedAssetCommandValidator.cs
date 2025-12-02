namespace Accounting.Application.FixedAssets.Reject.v1;

/// <summary>
/// Validator for RejectFixedAssetCommand.
/// </summary>
public sealed class RejectFixedAssetCommandValidator : AbstractValidator<RejectFixedAssetCommand>
{
    public RejectFixedAssetCommandValidator()
    {
        RuleFor(x => x.FixedAssetId).NotEmpty();
        RuleFor(x => x.RejectedBy)
            .NotEmpty()
            .MaximumLength(256);
        RuleFor(x => x.Reason)
            .MaximumLength(512);
    }
}


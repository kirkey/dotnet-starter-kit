namespace Accounting.Application.FixedAssets.Approve.v1;

/// <summary>
/// Validator for ApproveFixedAssetCommand.
/// </summary>
public sealed class ApproveFixedAssetCommandValidator : AbstractValidator<ApproveFixedAssetCommand>
{
    public ApproveFixedAssetCommandValidator()
    {
        RuleFor(x => x.FixedAssetId).NotEmpty();
        RuleFor(x => x.ApprovedBy)
            .NotEmpty()
            .MaximumLength(256);
    }
}


namespace Accounting.Application.FixedAssets.Approve;

/// <summary>
/// Validator for ApproveFixedAssetCommand.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApproveFixedAssetCommandValidator : AbstractValidator<ApproveFixedAssetCommand>
{
    public ApproveFixedAssetCommandValidator()
    {
        RuleFor(x => x.FixedAssetId)
            .NotEmpty()
            .WithMessage("Fixed Asset ID is required.");
    }
}


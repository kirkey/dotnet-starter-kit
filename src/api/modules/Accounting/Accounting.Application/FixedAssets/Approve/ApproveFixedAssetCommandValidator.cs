using FluentValidation;

namespace Accounting.Application.FixedAssets.Approve;

/// <summary>
/// Validator for ApproveFixedAssetCommand.
/// </summary>
public sealed class ApproveFixedAssetCommandValidator : AbstractValidator<ApproveFixedAssetCommand>
{
    public ApproveFixedAssetCommandValidator()
    {
        RuleFor(x => x.FixedAssetId)
            .NotEmpty()
            .WithMessage("Fixed Asset ID is required.");

        RuleFor(x => x.ApprovedBy)
            .NotEmpty()
            .WithMessage("Approver is required.")
            .MaximumLength(200)
            .WithMessage("Approver name cannot exceed 200 characters.");
    }
}


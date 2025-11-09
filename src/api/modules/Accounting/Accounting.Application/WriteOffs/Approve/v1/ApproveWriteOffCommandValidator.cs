namespace Accounting.Application.WriteOffs.Approve.v1;

/// <summary>
/// Validator for ApproveWriteOffCommand.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApproveWriteOffCommandValidator : AbstractValidator<ApproveWriteOffCommand>
{
    public ApproveWriteOffCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Write-off ID is required.");
    }
}


namespace Accounting.Application.Bills.Approve.v1;

/// <summary>
/// Validator for ApproveBillCommand.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApproveBillCommandValidator : AbstractValidator<ApproveBillCommand>
{
    public ApproveBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("Bill ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Bill ID must be a valid identifier.");
    }
}

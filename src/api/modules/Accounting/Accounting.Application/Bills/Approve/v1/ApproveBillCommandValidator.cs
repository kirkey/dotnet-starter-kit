namespace Accounting.Application.Bills.Approve.v1;

/// <summary>
/// Validator for ApproveBillCommand.
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

        RuleFor(x => x.ApprovedBy)
            .NotEmpty()
            .WithMessage("Approved by is required.")
            .MaximumLength(100)
            .WithMessage("Approved by cannot exceed 100 characters.");
    }
}

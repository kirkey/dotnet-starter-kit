namespace Accounting.Application.Bills.Void.v1;

/// <summary>
/// Validator for VoidBillCommand.
/// </summary>
public sealed class VoidBillCommandValidator : AbstractValidator<VoidBillCommand>
{
    public VoidBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("Bill ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Bill ID must be a valid identifier.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Void reason is required.")
            .MaximumLength(512)
            .WithMessage("Void reason cannot exceed 500 characters.");
    }
}

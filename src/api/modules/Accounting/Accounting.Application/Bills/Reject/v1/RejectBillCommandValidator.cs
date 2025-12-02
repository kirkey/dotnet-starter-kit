namespace Accounting.Application.Bills.Reject.v1;

/// <summary>
/// Validator for RejectBillCommand.
/// </summary>
public sealed class RejectBillCommandValidator : AbstractValidator<RejectBillCommand>
{
    public RejectBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("Bill ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Bill ID must be a valid identifier.");

        RuleFor(x => x.RejectedBy)
            .NotEmpty()
            .WithMessage("Rejected by is required.")
            .MaximumLength(128)
            .WithMessage("Rejected by cannot exceed 100 characters.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Rejection reason is required.")
            .MaximumLength(512)
            .WithMessage("Rejection reason cannot exceed 500 characters.");
    }
}

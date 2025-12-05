namespace Accounting.Application.Checks.StopPayment.v1;

/// <summary>
/// Validator for stop payment command.
/// </summary>
public class StopPaymentCheckCommandValidator : AbstractValidator<StopPaymentCheckCommand>
{
    public StopPaymentCheckCommandValidator()
    {
        RuleFor(x => x.CheckId)
            .NotEmpty().WithMessage("Check ID is required.");

        RuleFor(x => x.StopPaymentReason)
            .NotEmpty().WithMessage("Stop payment reason is required.")
            .MaximumLength(512).WithMessage("Stop payment reason cannot exceed 512 characters.")
            .MinimumLength(10).WithMessage("Stop payment reason must be at least 10 characters.");

        RuleFor(x => x.StopPaymentDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Stop payment date cannot be in the future.")
            .When(x => x.StopPaymentDate.HasValue);
    }
}


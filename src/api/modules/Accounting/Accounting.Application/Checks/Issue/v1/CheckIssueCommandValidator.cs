namespace Accounting.Application.Checks.Issue.v1;

/// <summary>
/// Validator for check issue command with strict validation rules.
/// </summary>
public class CheckIssueCommandValidator : AbstractValidator<CheckIssueCommand>
{
    public CheckIssueCommandValidator()
    {
        RuleFor(x => x.CheckId)
            .NotEmpty().WithMessage("Check ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Check amount must be greater than zero.")
            .LessThanOrEqualTo(9999999999.99m).WithMessage("Check amount cannot exceed 9,999,999,999.99.");

        RuleFor(x => x.PayeeName)
            .NotEmpty().WithMessage("Payee name is required.")
            .MaximumLength(256).WithMessage("Payee name cannot exceed 256 characters.")
            .Matches(@"^[a-zA-Z0-9\s\.\,\-\'&]+$").WithMessage("Payee name contains invalid characters.");

        RuleFor(x => x.IssuedDate)
            .NotEmpty().WithMessage("Issued date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).WithMessage("Issued date cannot be more than 1 day in the future.");

        RuleFor(x => x.Memo)
            .MaximumLength(512).WithMessage("Memo cannot exceed 512 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Memo));

        RuleFor(x => x)
            .Must(x => x.PayeeId.HasValue || x.VendorId.HasValue)
            .WithMessage("Either Payee ID or Vendor ID must be provided.")
            .When(x => x.PaymentId.HasValue || x.ExpenseId.HasValue);
    }
}


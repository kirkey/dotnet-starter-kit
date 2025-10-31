namespace Accounting.Application.PrepaidExpenses.Create.v1;

/// <summary>
/// Validator for prepaid expense creation command.
/// </summary>
public class PrepaidExpenseCreateCommandValidator : AbstractValidator<PrepaidExpenseCreateCommand>
{
    public PrepaidExpenseCreateCommandValidator()
    {
        RuleFor(x => x.PrepaidNumber)
            .NotEmpty().WithMessage("Prepaid number is required")
            .MaximumLength(50).WithMessage("Prepaid number cannot exceed 50 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be positive");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .LessThan(x => x.EndDate).WithMessage("Start date must be before end date");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(x => x.PrepaidAssetAccountId)
            .NotEmpty().WithMessage("Prepaid asset account ID is required");

        RuleFor(x => x.ExpenseAccountId)
            .NotEmpty().WithMessage("Expense account ID is required");

        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("Payment date is required");

        RuleFor(x => x.AmortizationSchedule)
            .NotEmpty().WithMessage("Amortization schedule is required")
            .MaximumLength(32).WithMessage("Amortization schedule cannot exceed 32 characters")
            .Must(schedule => new[] { "Monthly", "Quarterly", "Annually", "Custom" }.Contains(schedule))
            .WithMessage("Amortization schedule must be one of: Monthly, Quarterly, Annually, Custom");

        RuleFor(x => x.VendorName)
            .MaximumLength(256).WithMessage("Vendor name cannot exceed 256 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.VendorName));


        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}


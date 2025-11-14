namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Create.v1;

public class CreatePayrollDeductionValidator : AbstractValidator<CreatePayrollDeductionCommand>
{
    public CreatePayrollDeductionValidator()
    {
        RuleFor(x => x.PayComponentId)
            .NotEmpty().WithMessage("Pay component ID is required.");

        RuleFor(x => x.DeductionType)
            .NotEmpty().WithMessage("Deduction type is required.")
            .Must(type => new[] { "FixedAmount", "Percentage", "Monthly", "PerPayPeriod" }.Contains(type))
            .WithMessage("Deduction type must be one of: FixedAmount, Percentage, Monthly, PerPayPeriod.");

        RuleFor(x => x.DeductionAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Deduction amount cannot be negative.");

        RuleFor(x => x.DeductionPercentage)
            .GreaterThanOrEqualTo(0).WithMessage("Deduction percentage cannot be negative.")
            .LessThanOrEqualTo(100).WithMessage("Deduction percentage cannot exceed 100%.");

        RuleFor(x => x)
            .Must(x => x.EmployeeId.HasValue || x.OrganizationalUnitId.HasValue || (!x.EmployeeId.HasValue && !x.OrganizationalUnitId.HasValue))
            .WithMessage("Deduction can apply to individual employee, organizational unit, or company-wide (both null).");

        RuleFor(x => x.MaxDeductionLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Max deduction limit cannot be negative.")
            .When(x => x.MaxDeductionLimit.HasValue);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.")
            .When(x => x.EndDate.HasValue && x.StartDate.HasValue);

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(100).WithMessage("Reference number must not exceed 100 characters.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1000).WithMessage("Remarks must not exceed 1000 characters.");
    }
}


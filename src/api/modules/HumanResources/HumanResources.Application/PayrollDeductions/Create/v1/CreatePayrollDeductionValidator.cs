namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Create.v1;

/// <summary>
/// Validator for CreatePayrollDeductionCommand with Philippines Labor Code compliance.
/// </summary>
public class CreatePayrollDeductionValidator : AbstractValidator<CreatePayrollDeductionCommand>
{
    public CreatePayrollDeductionValidator()
    {
        RuleFor(x => x.PayComponentId)
            .NotEmpty().WithMessage("Pay component ID is required.");

        RuleFor(x => x.DeductionType)
            .NotEmpty().WithMessage("Deduction type is required.")
            .Must(t => new[] { "FixedAmount", "Percentage", "Monthly", "PerPayPeriod" }.Contains(t))
            .WithMessage("Deduction type must be: FixedAmount, Percentage, Monthly, or PerPayPeriod.");

        RuleFor(x => x.DeductionAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Deduction amount cannot be negative.")
            .When(x => x.DeductionType is "FixedAmount" or "Monthly" or "PerPayPeriod");

        RuleFor(x => x.DeductionPercentage)
            .GreaterThanOrEqualTo(0).WithMessage("Deduction percentage cannot be negative.")
            .LessThanOrEqualTo(100).WithMessage("Deduction percentage cannot exceed 100%.")
            .When(x => x.DeductionType == "Percentage");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .LessThanOrEqualTo(DateTime.Today.AddMonths(3))
            .WithMessage("Start date cannot be more than 3 months in the future.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.MaxDeductionLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Max deduction limit cannot be negative.")
            .LessThanOrEqualTo(1000000).WithMessage("Max deduction limit must be reasonable.")
            .When(x => x.MaxDeductionLimit.HasValue);

        // Philippines Labor Code: At least one of EmployeeId or OrganizationalUnitId
        RuleFor(x => x)
            .Must(x => x.EmployeeId.HasValue || x.OrganizationalUnitId.HasValue)
            .WithMessage("Either EmployeeId or OrganizationalUnitId must be specified.");

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(100).WithMessage("Reference number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.Remarks)
            .MaximumLength(500).WithMessage("Remarks must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));

        // Philippines Labor Code Art 113: Total deductions cannot exceed 70% of wages
        RuleFor(x => x.MaxDeductionLimit)
            .LessThanOrEqualTo(0.7m).WithMessage("Max deduction limit cannot exceed 70% of wages per Labor Code Art 113.")
            .When(x => x.DeductionType == "Percentage");
    }
}


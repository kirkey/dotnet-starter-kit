namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Create.v1;

public class CreateEmployeePayComponentValidator : AbstractValidator<CreateEmployeePayComponentCommand>
{
    public CreateEmployeePayComponentValidator()
    {
        RuleFor(cmd => cmd.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(cmd => cmd.PayComponentId)
            .NotEmpty().WithMessage("Pay component ID is required.");

        RuleFor(cmd => cmd.AssignmentType)
            .NotEmpty().WithMessage("Assignment type is required.")
            .Must(type => new[] { "Standard", "Override", "Addition", "OneTime" }.Contains(type))
            .WithMessage("Assignment type must be: Standard, Override, Addition, or OneTime.");

        RuleFor(cmd => cmd.CustomRate)
            .GreaterThanOrEqualTo(0m).WithMessage("Custom rate cannot be negative.")
            .When(cmd => cmd.CustomRate.HasValue);

        RuleFor(cmd => cmd.FixedAmount)
            .GreaterThanOrEqualTo(0m).WithMessage("Fixed amount cannot be negative.")
            .When(cmd => cmd.FixedAmount.HasValue);

        RuleFor(cmd => cmd.TotalAmount)
            .GreaterThanOrEqualTo(0m).WithMessage("Total amount cannot be negative.")
            .When(cmd => cmd.TotalAmount.HasValue);

        RuleFor(cmd => cmd.InstallmentCount)
            .GreaterThan(0).WithMessage("Installment count must be greater than 0.")
            .When(cmd => cmd.InstallmentCount.HasValue);

        RuleFor(cmd => cmd.CustomFormula)
            .MaximumLength(500).WithMessage("Custom formula must not exceed 500 characters.");

        RuleFor(cmd => cmd.ReferenceNumber)
            .MaximumLength(100).WithMessage("Reference number must not exceed 100 characters.");

        RuleFor(cmd => cmd.Remarks)
            .MaximumLength(1000).WithMessage("Remarks must not exceed 1000 characters.");
    }
}


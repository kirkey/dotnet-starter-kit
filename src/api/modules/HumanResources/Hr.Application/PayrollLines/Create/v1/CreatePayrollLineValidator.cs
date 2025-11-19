namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Create.v1;

/// <summary>
/// Validator for creating a payroll line.
/// </summary>
public class CreatePayrollLineValidator : AbstractValidator<CreatePayrollLineCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePayrollLineValidator"/> class.
    /// </summary>
    public CreatePayrollLineValidator()
    {
        RuleFor(x => x.PayrollId)
            .NotEmpty()
            .WithMessage("Payroll ID is required");

        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.RegularHours)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Regular hours cannot be negative")
            .LessThanOrEqualTo(260)
            .WithMessage("Regular hours cannot exceed 260 per month");

        RuleFor(x => x.OvertimeHours)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Overtime hours cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Overtime hours cannot exceed 100 per month");
    }
}


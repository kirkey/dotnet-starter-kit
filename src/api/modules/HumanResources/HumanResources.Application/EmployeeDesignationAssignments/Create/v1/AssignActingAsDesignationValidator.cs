namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Create.v1;

/// <summary>
/// Validator for AssignActingAsDesignationCommand.
/// </summary>
public class AssignActingAsDesignationValidator : AbstractValidator<AssignActingAsDesignationCommand>
{
    public AssignActingAsDesignationValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.DesignationId)
            .NotEmpty().WithMessage("Designation ID is required.");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty().WithMessage("Effective date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Effective date cannot be in the future.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.EffectiveDate).WithMessage("End date must be after effective date.")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.AdjustedSalary)
            .GreaterThan(0).WithMessage("Adjusted salary must be greater than zero.")
            .When(x => x.AdjustedSalary.HasValue);

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Create.v1;

/// <summary>
/// Validator for AssignPlantillaDesignationCommand.
/// </summary>
public class AssignPlantillaDesignationValidator : AbstractValidator<AssignPlantillaDesignationCommand>
{
    public AssignPlantillaDesignationValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.DesignationId)
            .NotEmpty().WithMessage("Designation ID is required.");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty().WithMessage("Effective date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Effective date cannot be in the future.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}


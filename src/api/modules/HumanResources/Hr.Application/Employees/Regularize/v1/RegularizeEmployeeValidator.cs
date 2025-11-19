namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Regularize.v1;

/// <summary>
/// Validator for RegularizeEmployeeCommand.
/// </summary>
public class RegularizeEmployeeValidator : AbstractValidator<RegularizeEmployeeCommand>
{
    public RegularizeEmployeeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.RegularizationDate)
            .NotEmpty().WithMessage("Regularization date is required.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Regularization date cannot be in the future.");
    }
}


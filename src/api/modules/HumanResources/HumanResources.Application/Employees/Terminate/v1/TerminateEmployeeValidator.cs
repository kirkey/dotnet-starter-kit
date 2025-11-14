namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Terminate.v1;

/// <summary>
/// Validator for TerminateEmployeeCommand with Philippines Labor Code compliance.
/// </summary>
public class TerminateEmployeeValidator : AbstractValidator<TerminateEmployeeCommand>
{
    public TerminateEmployeeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.TerminationDate)
            .NotEmpty().WithMessage("Termination date is required.")
            .LessThanOrEqualTo(DateTime.Today.AddMonths(3))
            .WithMessage("Termination date cannot be more than 3 months in the future.");

        RuleFor(x => x.TerminationReason)
            .NotEmpty().WithMessage("Termination reason is required.")
            .Must(reason => new[]
            {
                // Authorized causes
                "ReductionOfWorkforce", "Redundancy", "BusinessClosure",
                // Just causes
                "MisconductJustCause", "NeglectOfDuty", "BreachOfTrust", "CriminalOffense", "HabitualAbsenteeism",
                // Voluntary
                "ResignationVoluntary", "Retirement", "EndOfContract", "Death", "ProbationNotConfirmed"
            }.Contains(reason))
            .WithMessage("Invalid termination reason per Labor Code.");

        RuleFor(x => x.TerminationMode)
            .NotEmpty().WithMessage("Termination mode is required.")
            .Must(mode => new[] { "ByEmployer", "ByEmployee", "MutualConsent", "ByOperationOfLaw" }.Contains(mode))
            .WithMessage("Termination mode must be: ByEmployer, ByEmployee, MutualConsent, or ByOperationOfLaw.");

        RuleFor(x => x.SeparationPayBasis)
            .Must(basis => basis == null || new[] { "HalfMonthPerYear", "OneMonthPerYear", "None", "CustomAmount" }.Contains(basis))
            .WithMessage("Separation pay basis must be: HalfMonthPerYear, OneMonthPerYear, None, or CustomAmount.");

        RuleFor(x => x.SeparationPayAmount)
            .GreaterThan(0).WithMessage("Separation pay amount must be greater than zero.")
            .When(x => x.SeparationPayAmount.HasValue);
    }
}


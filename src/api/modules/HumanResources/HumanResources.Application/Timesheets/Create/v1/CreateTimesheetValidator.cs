namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Create.v1;

public class CreateTimesheetValidator : AbstractValidator<CreateTimesheetCommand>
{
    public CreateTimesheetValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Start date cannot be in the future.")
            .GreaterThanOrEqualTo(DateTime.Today.AddYears(-1)).WithMessage("Start date cannot be more than 1 year old.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("End date cannot be in the future.");

        RuleFor(x => x.PeriodType)
            .NotEmpty().WithMessage("Period type is required.")
            .Must(BeValidPeriodType).WithMessage("Period type must be one of: Weekly, BiWeekly, Monthly.");
    }

    private static bool BeValidPeriodType(string? periodType)
    {
        return periodType is "Weekly" or "BiWeekly" or "Monthly";
    }
}


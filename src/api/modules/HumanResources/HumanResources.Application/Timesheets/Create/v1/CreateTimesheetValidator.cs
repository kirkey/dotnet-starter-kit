namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Create.v1;

public class CreateTimesheetValidator : AbstractValidator<CreateTimesheetCommand>
{
    public CreateTimesheetValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.PeriodType)
            .NotEmpty()
            .WithMessage("Period type is required")
            .Must(BeValidPeriodType)
            .WithMessage("Period type must be Weekly, BiWeekly, or Monthly");
    }

    private static bool BeValidPeriodType(string? periodType)
    {
        if (string.IsNullOrWhiteSpace(periodType))
            return false;

        var validTypes = new[] { "Weekly", "BiWeekly", "Monthly" };
        return validTypes.Contains(periodType);
    }
}


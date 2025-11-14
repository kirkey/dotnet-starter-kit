namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Accrue.v1;

/// <summary>
/// Validator for AccrueLeaveCommand.
/// </summary>
public class AccrueLeaveValidator : AbstractValidator<AccrueLeaveCommand>
{
    public AccrueLeaveValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty().WithMessage("Leave type ID is required.");

        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(2020).WithMessage("Year must be 2020 or later.")
            .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("Year cannot be more than 1 year in the future.");

        RuleFor(x => x.DaysToAccrue)
            .GreaterThan(0).WithMessage("Days to accrue must be greater than 0.")
            .LessThanOrEqualTo(365).WithMessage("Days to accrue cannot exceed 365 days.");
    }
}


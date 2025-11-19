namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Create.v1;

/// <summary>
/// Validator for creating an attendance record.
/// </summary>
public class CreateAttendanceValidator : AbstractValidator<CreateAttendanceCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAttendanceValidator"/> class.
    /// </summary>
    public CreateAttendanceValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.ClockInTime)
            .NotEmpty()
            .WithMessage("Clock in time is required");

        RuleFor(x => x.ClockInLocation)
            .MaximumLength(250)
            .WithMessage("Clock in location cannot exceed 250 characters");
    }
}

namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

public class UpdateTimesheetValidator : AbstractValidator<UpdateTimesheetCommand>
{
    public UpdateTimesheetValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.Status)
            .Must(BeValidStatus).WithMessage("Status must be one of: Submitted, Approved, Rejected, Locked.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.ManagerComment)
            .MaximumLength(1000).WithMessage("Manager comment must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ManagerComment));
    }

    private static bool BeValidStatus(string? status)
    {
        return status is "Submitted" or "Approved" or "Rejected" or "Locked";
    }
}


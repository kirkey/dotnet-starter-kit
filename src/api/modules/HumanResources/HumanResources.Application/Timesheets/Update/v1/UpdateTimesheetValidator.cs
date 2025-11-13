namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

public class UpdateTimesheetValidator : AbstractValidator<UpdateTimesheetCommand>
{
    public UpdateTimesheetValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.ManagerComment)
            .MaximumLength(500).WithMessage("Manager comment must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ManagerComment));

        RuleFor(x => x.RejectionReason)
            .MaximumLength(500).WithMessage("Rejection reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.RejectionReason));

        RuleFor(x => x.Status)
            .Must(BeValidStatus).WithMessage("Status must be one of: Draft, Submitted, Approved, Rejected, Locked.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));
    }

    private static bool BeValidStatus(string status)
    {
        return status is "Draft" or "Submitted" or "Approved" or "Rejected" or "Locked";
    }
}


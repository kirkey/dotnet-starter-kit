namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Create.v1;

public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required");

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty().WithMessage("Leave type ID is required");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date");

        RuleFor(x => x.Reason)
            .MaximumLength(512).WithMessage("Reason cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}


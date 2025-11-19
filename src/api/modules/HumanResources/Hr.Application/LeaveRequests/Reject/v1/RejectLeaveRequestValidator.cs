namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Reject.v1;

/// <summary>
/// Validator for RejectLeaveRequestCommand.
/// </summary>
public class RejectLeaveRequestValidator : AbstractValidator<RejectLeaveRequestCommand>
{
    public RejectLeaveRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Leave request ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Rejection reason is required.")
            .MaximumLength(500).WithMessage("Rejection reason must not exceed 500 characters.");
    }
}


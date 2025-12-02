namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Approve.v1;

/// <summary>
/// Validator for ApproveLeaveRequestCommand.
/// </summary>
public class ApproveLeaveRequestValidator : AbstractValidator<ApproveLeaveRequestCommand>
{
    public ApproveLeaveRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Leave request ID is required.");

        RuleFor(x => x.Comment)
            .MaximumLength(512).WithMessage("Comment must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}


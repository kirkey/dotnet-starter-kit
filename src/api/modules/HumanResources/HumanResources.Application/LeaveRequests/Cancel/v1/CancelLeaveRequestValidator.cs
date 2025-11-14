namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Cancel.v1;

/// <summary>
/// Validator for CancelLeaveRequestCommand.
/// </summary>
public class CancelLeaveRequestValidator : AbstractValidator<CancelLeaveRequestCommand>
{
    public CancelLeaveRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Leave request ID is required.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Cancellation reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}


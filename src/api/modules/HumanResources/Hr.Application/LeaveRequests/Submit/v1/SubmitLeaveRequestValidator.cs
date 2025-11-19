namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Submit.v1;

/// <summary>
/// Validator for SubmitLeaveRequestCommand.
/// </summary>
public class SubmitLeaveRequestValidator : AbstractValidator<SubmitLeaveRequestCommand>
{
    public SubmitLeaveRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Leave request ID is required.");

        RuleFor(x => x.ApproverManagerId)
            .NotEmpty().WithMessage("Approver manager ID is required.");
    }
}


namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Update.v1;

public class UpdateLeaveRequestValidator : AbstractValidator<UpdateLeaveRequestCommand>
{
    public UpdateLeaveRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Leave request ID is required");

        RuleFor(x => x.Status)
            .Must(BeValidStatus)
            .When(x => !string.IsNullOrWhiteSpace(x.Status))
            .WithMessage("Status must be Approved, Rejected, or Cancelled");

        RuleFor(x => x.ApproverComment)
            .MaximumLength(512).WithMessage("Approver comment cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ApproverComment));
    }

    private static bool BeValidStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return true;

        var validStatuses = new[] { "Approved", "Rejected", "Cancelled" };
        return validStatuses.Contains(status);
    }
}


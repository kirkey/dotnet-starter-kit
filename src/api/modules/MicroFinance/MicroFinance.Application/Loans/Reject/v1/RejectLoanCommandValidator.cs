using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Reject.v1;

public sealed class RejectLoanCommandValidator : AbstractValidator<RejectLoanCommand>
{
    public RejectLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Loan ID is required.");

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .MaximumLength(512)
            .WithMessage("Rejection reason is required and must not exceed 512 characters.");
    }
}
